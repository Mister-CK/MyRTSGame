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
            base.ExecuteJob();
            if (CurrentJob is not BuilderJob) return;

            Destination.SetState(new CompletedState(Destination.BuildingType));
            HasDestination = false;
        }

        public void UnAssignBuilderJob()
        {
            CurrentJob = null;
            Destination = null;
            HasDestination = false;

            SetDestination();
        }
    }
}