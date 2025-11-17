using Interface;
using Domain.Model;
using Domain.Units.Component;

namespace Domain.Units.JobExecutors{
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
