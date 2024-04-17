using UnityEngine;

namespace MyRTSGame.Model
{
    public class Builder : Unit
    {
        [SerializeField] private BuilderJobQueue builderJobQueue;
        private BuilderJob _currentBuilderJob;
        
        public Builder()
        {
            UnitType = UnitType.Builder;
        }
        
        protected override void ExecuteJob()
        {
            Destination.SetState(new CompletedState(Destination.BuildingType));
            
            HasDestination = false;
        }
        
        private void RequestNewJob()
        {
            unitController.CreateBuilderJobRequest(this);
        }
        
        public void AcceptNewBuilderJob(BuilderJob builderJob)
        {
            _currentBuilderJob = builderJob;
            Destination = _currentBuilderJob.Destination;
            HasDestination = true;
            Agent.SetDestination(Destination.transform.position);
        }

        public void UnAssignBuilderJob()
        {
            _currentBuilderJob = null;
            Destination = null;
            HasDestination = false;
            SetDestination();
        }
        
        protected override void SetDestination()
        {
            RequestNewJob();
        }
    }
}