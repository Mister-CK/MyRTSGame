using Interface;
using MyRTSGame.Model;
using Units.Model.Component;

namespace Units.Model.JobExecutors{
    public class LookingForBuildingExecutor: IJobExecutor
    {
        public void Execute(UnitComponent unitComponent, Job job)
        {
            var lookingForBuildingJob = (LookingForBuildingJob)job;
            unitComponent.HandleLookingForBuildingJob(lookingForBuildingJob);
                
            unitComponent.OnJobCompleted?.Invoke(lookingForBuildingJob);
            unitComponent.Data.SetIsLookingForBuilding(false);
            unitComponent.Data.ResetJobState();
        }
    }
}
