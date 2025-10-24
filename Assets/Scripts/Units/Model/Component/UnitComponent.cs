using Application;
using Enums;
using Interface;
using MyRTSGame.Model;
using System;
using Units.Model.Data;
using UnityEngine;
using UnityEngine.AI;

namespace Units.Model.Component
{
    public abstract class UnitComponent : MonoBehaviour, ISelectable
    {
        public UnitService unitService;
        
        public UnitData Data { get; private set; }
        protected NavMeshAgent Agent;
        protected abstract JobType DefaultJobType { get; }
        
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

        protected virtual void HandleLookingForBuildingJob(LookingForBuildingJob job) { }

        protected virtual void HandleUnAssignCleanup(DestinationType destinationType) { }
        protected virtual void ExecuteJob()
        {
            if (Data.CurrentJob is LookingForBuildingJob lookingForBuildingJob)
            {
                HandleLookingForBuildingJob(lookingForBuildingJob);
                
                unitService.CompleteJob(lookingForBuildingJob);
                Data.SetIsLookingForBuilding(false);
                Data.ResetJobState();
                return;
            }

            if (Data.CurrentJob is ConsumptionJob consumptionJob)
            {
                unitService.RemoveResourceFromDestination(consumptionJob.Destination, consumptionJob.ResourceType, 1);
                
                Data.ResetJobState();
                Data.ReplenishStamina();
                Data.SetRequestedConsumptionJob(false);
                return;
            }
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