using Enums;
using Interface;
using MyRTSGame.Model;

namespace Units.Model.Data
{
    public class UnitData
    {
        private float _stamina  = 100f;

        public UnitType UnitType { get; private set; } 
        public Job CurrentJob { get; private set; }
        public IDestination Destination { get; private set; }
        
        public bool HasDestination { get; private set; }
        public bool HasJobToExecute { get; private set; } = true;
        public bool IsLookingForBuilding { get; private set; } = false;
        public bool HasPendingJobRequest { get; private set; }
        public bool HasRequestedConsumptionJob { get; private set; }
        
        protected void SetUnitType(UnitType unitType)
        {
            UnitType = unitType;
        }
        
        public void SetStamina(float stamina)
        {
            _stamina = stamina;
        }
        
        public float GetStamina()
        {
            return _stamina;
        }
        
        public void DecrementStamina(float amount)
        {
            _stamina -= amount;
            if (_stamina < 0) _stamina = 0;
        }
        
        public void ReplenishStamina()
        {
            _stamina = 100f;
        }
        
        public void SetIsLookingForBuilding(bool isLookingForBuilding)
        {
            IsLookingForBuilding = isLookingForBuilding;
        }
        
        public void SetCurrentJob(Job job)
        {
            CurrentJob = job;
            Destination = job?.Destination;
        }

        public void SetDestination(IDestination destination)
        {
            Destination = destination;
            HasDestination = (destination != null);
        }

        public void SetPendingJobRequest(bool isPending)
        {
            HasPendingJobRequest = isPending;
        }

        public void SetHasJobToExecute(bool hasJobToExecute)
        {
            HasJobToExecute = hasJobToExecute;
        }
        
        public void SetRequestedConsumptionJob(bool hasRequested)
        {
            HasRequestedConsumptionJob = hasRequested;
        }
        
        public void SetHasDestination(bool hasDestination)
        {
            HasDestination = hasDestination;
        }
        
        public void ResetJobState()
        {
            HasDestination = false;
            CurrentJob = null;
            Destination = null;
        }
    }
}
