using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

namespace MyRTSGame.Model
{
    public abstract class Unit: MonoBehaviour, ISelectable
    {
        public UnitController unitController;
        protected NavMeshAgent Agent;
        protected bool HasDestination;
        protected IDestination Destination;
        protected UnitType UnitType;
        protected Job CurrentJob;
        private float _stamina;
        private bool _hasPendingJobRequest;
        private bool _hasRequestedConsumptionJob;
        protected bool IsLookingForBuilding = false;
        
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
            Agent = GetComponentInChildren<NavMeshAgent>();
            unitController = UnitController.Instance;
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
                CheckIfDestinationIsReached();
            else
                SetDestination();
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
                        ExecuteJob();
                    }
                }
            }
        }

        protected virtual void ExecuteJob()
        {
            if (CurrentJob is LookingForBuildingJob lookingForBuildingJob)
            {
                if (this is not ResourceCollector resourceCollector) throw new ArgumentException("only resourceCollectors can look for buildings");
                resourceCollector.SetBuilding(lookingForBuildingJob.Destination as Building);
                IsLookingForBuilding = false;
                HasDestination = false;
                CurrentJob = null;
                Destination = null;
                
                return;
            }

            if (CurrentJob is ConsumptionJob consumptionJob)
            {
                unitController.RemoveResourceFromDestination(consumptionJob.Destination, consumptionJob.ResourceType, 1);
                HasDestination = false;
                CurrentJob = null;
                Destination = null;
                _stamina = 100;
                return;
            }
        }
        protected void SetDestination()
        {
            if (_hasPendingJobRequest) return;
            if (_stamina < 30 && !_hasRequestedConsumptionJob)
            {
                unitController.CreateUnitJobRequest(this, JobType.ConsumptionJob);
                _hasRequestedConsumptionJob = true;
                return;
            }

            if (IsLookingForBuilding)
            {
                unitController.CreateNewLookForBuildingJob(this);
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

            unitController.CreateUnitJobRequest(this, jobType);
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

        private bool CheckIfJobNeedsToBeAddedBackToQueue()
        {
            return CurrentJob switch
            {
                null => false,
                ConsumptionJob => false,
                BuilderJob => true,
                LookingForBuildingJob => true,
                VillagerJob => this is Villager villager && !villager.GetHasResource(),
                CollectResourceJob => this is ResourceCollector resourceCollector && !resourceCollector.GetHasResource(),
                _ => throw new InvalidOperationException("Unknown job type")
            };
        }
        
        public void DeleteUnit()
        {
            if (CheckIfJobNeedsToBeAddedBackToQueue()) unitController.AddJobBackToQueue(CurrentJob);
            Destroy(gameObject);
        }
    }
}