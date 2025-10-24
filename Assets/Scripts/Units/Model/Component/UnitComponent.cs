using Application;
using Enums;
using Interface;
using MyRTSGame.Model;
using System;
using System.Collections.Generic;
using Units.Model.Data;
using Units.Model.JobExecutors;
using UnityEngine;
using UnityEngine.AI;

namespace Units.Model.Component
{
    public abstract class UnitComponent : MonoBehaviour, ISelectable
    {
        public UnitService unitService;
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

            if (ServiceInjector.Instance != null) ServiceInjector.Instance.InjectUnitDependencies(this);
            else Debug.LogError("ServiceInjector not found. UnitService will be null.");

            Agent = GetComponentInChildren<NavMeshAgent>();
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
                unitService.CreateUnitJobRequest(this, JobType.ConsumptionJob);
                Data.SetRequestedConsumptionJob(true);
                return;
            }

            if (Data.IsLookingForBuilding)
            {
                unitService.CreateNewLookForBuildingJob(this);
                return;
            }

            RequestNewJob();
            Data.SetRequestedConsumptionJob(false);
        }   
        
        private void RequestNewJob()
        {
            unitService.CreateUnitJobRequest(this, DefaultJobType);
        }
    }
}