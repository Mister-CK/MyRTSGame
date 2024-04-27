using UnityEngine;

namespace MyRTSGame.Model
{
    public class Builder : Unit
    {
        public Builder()
        {
            UnitType = UnitType.Builder;
        }
        
        protected override void ExecuteJob()
        {
            base.ExecuteJob();
            if (CurrentJob is not BuilderJob) return;

            Destination.SetState(new CompletedState(Destination.GetBuildingType()));
            unitController.CompleteJob(CurrentJob);
            HasDestination = false;
            CurrentJob = null;
            Destination = null;

        }
    }
}