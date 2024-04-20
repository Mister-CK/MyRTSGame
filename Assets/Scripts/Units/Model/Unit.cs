using UnityEngine;
using UnityEngine.AI;

namespace MyRTSGame.Model
{
    public abstract class Unit: MonoBehaviour, ISelectable
    {
        public UnitController unitController;
        protected NavMeshAgent Agent;
        protected bool HasDestination;
        protected Building Destination;
        protected UnitType UnitType;
        protected Job CurrentJob;
        private float _stamina;
        private bool _hasPendingJobRequest;
        private bool _hasRequestedConsumptionJob;
        
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
        
        private void Start()
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
            if (CurrentJob is ConsumptionJob consumptionJob)
            {
                unitController.RemoveResourceFromBuilding(consumptionJob.Destination, consumptionJob.ResourceType, 1);
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
                unitController.CreateConsumptionJobRequest(this);
                _hasRequestedConsumptionJob = true;
                return;
            }
            RequestNewJob();
            _hasRequestedConsumptionJob = false;
        }        
        private void RequestNewJob()
        {
            unitController.CreateUnitJobRequest(this);
        }

        public void AcceptNewConsumptionJob(ConsumptionJob consumptionJob)
        {
            SetPendingJobRequest(false);
            CurrentJob = consumptionJob;
            Destination = consumptionJob.Destination;
            Agent.SetDestination(Destination.transform.position);
            HasDestination = true;
        }
    }
}