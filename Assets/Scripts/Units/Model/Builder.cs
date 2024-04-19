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
        
        public void AcceptNewBuilderJob(Job job)
        {
            currentJob = job;
            Destination = currentJob.Destination;
            Agent.SetDestination(Destination.transform.position);
            HasDestination = true;
        }

        public void UnAssignBuilderJob()
        {
            currentJob = null;
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