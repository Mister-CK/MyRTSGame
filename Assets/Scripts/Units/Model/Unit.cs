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
            //
            // if (_stamina < 50)
            // {
            //     //get consumption job
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

        protected virtual void ExecuteJob() {}
        protected void SetDestination()
        {
            RequestNewJob();
        }        
        private void RequestNewJob()
        {
            unitController.CreateUnitJobRequest(this);
        }
    }
}