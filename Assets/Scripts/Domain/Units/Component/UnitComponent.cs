using Enums;
using Interface;
using Domain;
using Domain.Model;
using Navigation;
using System;
using System.Collections;
using System.Collections.Generic;
using Domain.Units.Data;
using Domain.Units.JobExecutors;
using UnityEngine;
using UnityEngine.AI;

namespace Domain.Units.Component
{
    public abstract class UnitComponent : MonoBehaviour, ISelectable
    {
        public Action<UnitComponent, JobType> OnRequestJob { get; set; }
        public Action<UnitComponent> OnRequestLookingForBuildingJob { get; set; }
        public Action<IDestination, ResourceType, int> OnRemoveResourceFromDestination { get; set; }

        public Action<Job> OnJobCompleted { get; set; }
        
        public UnitData Data { get; private set; }
        public NavMeshAgent Agent { get; private set; }
        protected abstract JobType DefaultJobType { get; }
        protected static readonly Dictionary<Type, IJobExecutor> JobExecutorsMap = new();
       
        static UnitComponent() 
        {
            JobExecutorsMap.Add(typeof(LookingForBuildingJob), new LookingForBuildingExecutor());
            JobExecutorsMap.Add(typeof(ConsumptionJob), new ConsumptionExecutor());
        }
        
        protected virtual void Awake()
        {
            var initialData = CreateUnitData();
            SetData(initialData);
        }

        protected virtual void Start()
        {
            Agent = GetComponentInChildren<NavMeshAgent>();
            StartCoroutine(InitializeAgentRoutine());
        }

        private IEnumerator InitializeAgentRoutine()
        {
            
            yield return new WaitUntil(() => NavMeshManager.IsNavMeshReady);
            if (Agent ==null) yield return null;
            var currentPosition = Agent.transform.position;
            var searchRadius = 2.0f; 

            if (NavMesh.SamplePosition(currentPosition, out NavMeshHit hit, searchRadius, NavMesh.AllAreas))
            {
                Agent.Warp(hit.position); 
            }
            else
            {
                Debug.LogError($"Unit failed to find NavMesh surface. Check NavMeshManager initialization and scene coverage.");
            }
            
        }
        
        protected void Update()
        {
            Data.DecrementStamina(Time.deltaTime);
            
            // if (_stamina < 0)
            // {
            //     Destroy(this);
            //     return;
            // }
            
            if (Data.HasDestination)
            {
                CheckIfDestinationIsReached();
            }
            else
            {
                SetDestination();
                Data.SetHasJobToExecute(true);
            }
        }
        protected abstract UnitData CreateUnitData();

        protected virtual void HandleJobAssignment(Job job) { }

        protected virtual void HandlePreDeletionCleanup() { }

        public virtual void HandleLookingForBuildingJob(LookingForBuildingJob job) { }

        protected virtual void HandleUnAssignCleanup(DestinationType destinationType) { }

        private void ExecuteJob()
        {
            if (Data.CurrentJob == null) return;
            var jobType = Data.CurrentJob.GetType();
            if (JobExecutorsMap.TryGetValue(jobType, out var executor)) executor.Execute(this, Data.CurrentJob);
            else Debug.Log("Executor not found for job type: " + jobType);
        }
        
        public void AcceptNewJob(Job job)
        {
            Data.SetPendingJobRequest(false);
            Data.SetCurrentJob(job);
            Data.SetDestination(job.Destination);
            Data.SetHasDestination(true);

            HandleJobAssignment(job);

            Agent.SetDestination(Data.Destination.GetPosition());
        }
        
        public void DeleteUnit()
        {
            HandlePreDeletionCleanup();
            Destroy(gameObject);
        }
        
        public void UnAssignJob(DestinationType destinationType)
        {
            HandleUnAssignCleanup(destinationType);
            Data.ResetJobState();
            Agent.SetDestination(Agent.transform.position);
        }
        
        private void SetData(UnitData unitData)
        {
            if (Data != null) return;
            Data = unitData;
        }

        private void CheckIfDestinationIsReached()
        {
            if (Agent.pathPending) return;
            if (Agent.remainingDistance > Agent.stoppingDistance) return;
            if (Agent.pathStatus != NavMeshPathStatus.PathComplete) return;
            if (Agent.hasPath && Agent.velocity.sqrMagnitude != 0f) return;
            if (!Data.HasJobToExecute) return;
            Data.SetHasJobToExecute(false);
            ExecuteJob();
        }
        
        private void SetDestination()
        {
            if (Data.HasPendingJobRequest) return;
            
            if (Data.GetStamina() < 30 && !Data.HasRequestedConsumptionJob)
            {
                OnRequestJob?.Invoke(this, JobType.ConsumptionJob);
                Data.SetRequestedConsumptionJob(true);
                return;
            }

            if (Data.IsLookingForBuilding)
            {
                OnRequestLookingForBuildingJob?.Invoke(this);
                return;
            }

            RequestNewJob();
            Data.SetRequestedConsumptionJob(false);
        }   
        
        private void RequestNewJob()
        {
            OnRequestJob?.Invoke(this, DefaultJobType);
        }
    }
}