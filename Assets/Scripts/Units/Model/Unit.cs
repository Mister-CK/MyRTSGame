using Application;
using Buildings.Model;
using Enums;
using Interface;
using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

namespace MyRTSGame.Model
{
    public abstract class Unit: MonoBehaviour, ISelectable
    {
        [FormerlySerializedAs("unitController")] public UnitService unitService;
        protected NavMeshAgent Agent;
        protected bool HasDestination;
        protected IDestination Destination;
        protected UnitType UnitType;
        protected Job CurrentJob;
        private float _stamina;
        private bool _hasPendingJobRequest;
        private bool _hasRequestedConsumptionJob;
        protected bool IsLookingForBuilding = false;
        protected bool HasJobToExecute = true;
        public void SetIsLookingForBuilding(bool isLookingForBuilding)
        {
            IsLookingForBuilding = isLookingForBuilding;
        }
        
        public void GetIsLookingForBuilding(bool isLookingForBuilding)
        {
            IsLookingForBuilding = isLookingForBuilding;
        }
        
        public Job GetCurrentJob()
        {
            return CurrentJob;
        }

        public void SetCurrentJob(Job job)
        {
            CurrentJob = job;
        }
        public void SetStamina(float stamina)
        {
            _stamina = stamina;
        }
        
        public float GetStamina()
        {
            return _stamina;
        }
        
        public UnitType GetUnitType()
        {
            return UnitType;
        }
        
        public void SetUnitType(UnitType unitType)
        {
            UnitType = unitType;
        }
        public bool GetPendingJobRequest()
        {
            return _hasPendingJobRequest;
        }
        
        public void SetPendingJobRequest(bool hasPendingJobRequest)
        {
            _hasPendingJobRequest = hasPendingJobRequest;
        }
        
        protected virtual void Start()
        {
            _stamina = 100;
            ServiceInjector.Instance.InjectUnitDependencies(this);

            Agent = GetComponentInChildren<NavMeshAgent>();
        }
        
        protected void Update()
        {
            _stamina -= Time.deltaTime;
            // if (_stamina < 0)
            // {
            //     //delete unit event
            //     Destroy(this);
            //     return;
            // }
            
            if (HasDestination)
            {
                CheckIfDestinationIsReached();
            }
            else
            {
                SetDestination();
                HasJobToExecute = true;
            }
        }

        private void CheckIfDestinationIsReached()
        {
            if (!Agent.pathPending)
            {
                if (Agent.remainingDistance <= Agent.stoppingDistance && Agent.pathStatus == NavMeshPathStatus.PathComplete)
                {
                    if (!Agent.hasPath || Agent.velocity.sqrMagnitude == 0f)
                    {
                        
                        // The agent has reached its destination
                        if (HasJobToExecute)
                        {
                            HasJobToExecute = false;
                            ExecuteJob();
                        }
                    }
                }
            }
        }

        protected virtual void ExecuteJob()
        {
            if (CurrentJob is LookingForBuildingJob lookingForBuildingJob)
            {
                if (this is not ResourceCollector resourceCollector) throw new ArgumentException("only resourceCollectors can look for buildings");
                if (lookingForBuildingJob.Destination is not Building building) return;
                resourceCollector.SetBuilding(building);
                resourceCollector.SetResourceTypeToCollect(building.OutputTypesWhenCompleted[0]);
                unitService.CompleteJob(lookingForBuildingJob);
                IsLookingForBuilding = false;
                HasDestination = false;
                CurrentJob = null;
                Destination = null;
                return;
            }

            if (CurrentJob is ConsumptionJob consumptionJob)
            {
                unitService.RemoveResourceFromDestination(consumptionJob.Destination, consumptionJob.ResourceType, 1);
                HasDestination = false;
                CurrentJob = null;
                Destination = null;
                _stamina = 100;
                return;
            }
        }

        private void SetDestination()
        {
            if (_hasPendingJobRequest) return;
            if (_stamina < 30 && !_hasRequestedConsumptionJob)
            {
                unitService.CreateUnitJobRequest(this, JobType.ConsumptionJob);
                _hasRequestedConsumptionJob = true;
                return;
            }

            if (IsLookingForBuilding)
            {
                unitService.CreateNewLookForBuildingJob(this);
                return;
            }

            RequestNewJob();
            _hasRequestedConsumptionJob = false;
        }   
        
        private void RequestNewJob()
        {
            var jobType = this switch
            {
                Builder => JobType.BuilderJob,
                Villager => JobType.VillagerJob,
                ResourceCollector => JobType.CollectResourceJob,
                _ => throw new ArgumentException("unit type not recognized in RequestNewJob")
            };

            unitService.CreateUnitJobRequest(this, jobType);
        }

        public void AcceptNewJob(Job job)
        {
            SetPendingJobRequest(false);
            CurrentJob = job;
            Destination = job.Destination;
            if (job is VillagerJob villagerJob) Destination = villagerJob.Origin; //todo refactor villagerJob to use Destination instead of Origin for first building.
            Agent.SetDestination(Destination.GetPosition());
            HasDestination = true;
        }
        
        public void DeleteUnit()
        {
            Destroy(gameObject);
            if (this is not ResourceCollector resourceCollector) return;
            unitService.CreateJobNeededEvent(JobType.LookForBuildingJob, resourceCollector.GetBuilding(), null, null, resourceCollector.GetBuilding().GetOccupantType());
        }
        
        public void UnAssignJob(DestinationType destinationType)
        {
            if (this is Villager villager)
            {
                if (destinationType == DestinationType.Origin && villager.GetHasResource())
                {
                    return;
                }
                villager.SetHasResource(false);
            }

            HasDestination = false;
            CurrentJob = null;
            Destination = null;
            Agent.SetDestination(Agent.transform.position);
        }
    }
}