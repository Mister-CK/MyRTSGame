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
        
        public UnitData Data { get; protected set; }
        
        protected NavMeshAgent Agent;
        
        protected virtual void Awake()
        {
            UnitData initialData = CreateUnitData();
            SetData(initialData);
        }

        protected virtual void Start()
        {

            if (ServiceInjector.Instance != null)
            {
                ServiceInjector.Instance.InjectUnitDependencies(this);
            }
            else
            {
                Debug.LogError("ServiceInjector not found. UnitService will be null.");
            }
            
            Agent = GetComponentInChildren<NavMeshAgent>();
        }
        
        protected void Update()
        {
            Data.DecrementStamina(Time.deltaTime);
            
            // if (_stamina < 0)
            // {
            //     //delete unit event
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
        
        protected void SetData(UnitData unitData)
        {
            if (Data != null)
            {
                Debug.LogError("Unit data is already set.");
                return;
            }
            Data = unitData;
        }

        private void CheckIfDestinationIsReached()
        {
            // ... (Movement logic remains the same) ...
            if (!Agent.pathPending)
            {
                if (Agent.remainingDistance <= Agent.stoppingDistance && Agent.pathStatus == NavMeshPathStatus.PathComplete)
                {
                    if (!Agent.hasPath || Agent.velocity.sqrMagnitude == 0f)
                    {
                        if (Data.HasJobToExecute)
                        {
                            Data.SetHasJobToExecute(false);
                            ExecuteJob();
                        }
                    }
                }
            }
        }

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
                // SERVICE CALL: Interact with the economy system
                unitService.RemoveResourceFromDestination(consumptionJob.Destination, consumptionJob.ResourceType, 1);
                
                // DATA UPDATE: Use POCO methods to reset state
                Data.ResetJobState();
                Data.ReplenishStamina();
                Data.SetRequestedConsumptionJob(false);
                return;
            }
        }
        protected virtual void HandleLookingForBuildingJob(LookingForBuildingJob job)
        {
            // Default implementation does nothing.
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
            var jobType = this switch
            {
                BuilderComponent => JobType.BuilderJob,
                VillagerComponent => JobType.VillagerJob,
                ResourceCollectorComponent => JobType.CollectResourceJob,
                _ => throw new ArgumentException("unit type not recognized in RequestNewJob")
            };

            unitService.CreateUnitJobRequest(this, jobType);
        }
        
        public void AcceptNewJob(Job job)
        {
            // DATA UPDATE
            Data.SetPendingJobRequest(false);
            Data.SetCurrentJob(job);
            Data.SetDestination(job.Destination);
            
            // Specific logic for job types (should be handled in specialized component)
            if (job is VillagerJob villagerJob) Data.SetDestination(villagerJob.Origin); 
            
            // ENGINE EXECUTION
            Agent.SetDestination(Data.Destination.GetPosition());
            Data.SetHasDestination(true);
        }

        public void DeleteUnit()
        {

            if (this is ResourceCollectorComponent resourceCollectorComponent)
            {
                var collectorData = (ResourceCollectorData)resourceCollectorComponent.Data;
                unitService.CreateJobNeededEvent(JobType.LookForBuildingJob, collectorData.GetBuilding(), null, null, collectorData.GetBuilding().GetOccupantType());
            }
            
            // ENGINE EXECUTION
            Destroy(gameObject);
        }
        
        public void UnAssignJob(DestinationType destinationType)
        {
            // Data logic for checking resource status (should be in ResourceCollectorComponent)
            if (this is VillagerComponent villager)
            {
                if (destinationType == DestinationType.Origin && villager.VillagerData.GetHasResource())
                {
                    return;
                }
                villager.VillagerData.SetHasResource(false);
            }
            
            // DATA UPDATE
            Data.ResetJobState();
            
            // ENGINE EXECUTION
            Agent.SetDestination(Agent.transform.position);
        }
    }
}