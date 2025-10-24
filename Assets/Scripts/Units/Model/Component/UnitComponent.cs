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
        
        protected virtual void Awake()
        {
            var initialData = CreateUnitData();
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
            if (Agent.pathPending) return;
            if (Agent.remainingDistance > Agent.stoppingDistance) return;
            if (Agent.pathStatus != NavMeshPathStatus.PathComplete) return;
            if (Agent.hasPath && Agent.velocity.sqrMagnitude != 0f) return;
            if (!Data.HasJobToExecute) return;
            Data.SetHasJobToExecute(false);
            ExecuteJob();
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
                unitService.RemoveResourceFromDestination(consumptionJob.Destination, consumptionJob.ResourceType, 1);
                
                Data.ResetJobState();
                Data.ReplenishStamina();
                Data.SetRequestedConsumptionJob(false);
                return;
            }
        }
        protected virtual void HandleLookingForBuildingJob(LookingForBuildingJob job) { }

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
            Data.SetPendingJobRequest(false);
            Data.SetCurrentJob(job);
            Data.SetDestination(job.Destination);
            
            if (job is VillagerJob villagerJob) Data.SetDestination(villagerJob.Origin); 
            
            Agent.SetDestination(Data.Destination.GetPosition());
            Data.SetHasDestination(true);
        }

        public void DeleteUnit()
        {

            if (this is ResourceCollectorComponent resourceCollectorComponent)
            {
                var collectorData = (ResourceCollectorData)resourceCollectorComponent.Data;
                unitService.CreateJobNeededEvent(JobType.LookForBuildingJob, collectorData.Building, null, null, collectorData.Building.GetOccupantType());
            }
            
            Destroy(gameObject);
        }
        
        public void UnAssignJob(DestinationType destinationType)
        {
            if (this is VillagerComponent villager)
            {
                if (destinationType == DestinationType.Origin && villager.VillagerData.GetHasResource())
                {
                    return;
                }
                villager.VillagerData.SetHasResource(false);
            }
            Data.ResetJobState();
            Agent.SetDestination(Agent.transform.position);
        }
    }
}