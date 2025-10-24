using Enums;
using Interface;
using MyRTSGame.Model;
using Units.Model.Component;

namespace Units.Model.JobExecutors
{
    public class CollectResourceExecutor : IJobExecutor
    {
        public void Execute(UnitComponent unit, Job job)
        {
            var rc = (ResourceCollectorComponent)unit;
            var rcJob = (CollectResourceJob)job;

            if (unit.Data.Destination != rc.CollectorData.Building && !rc.CollectorData.HasResource)
            {
                rc.TakeResource(rcJob.Destination, rc.CollectorData.ResourceTypeToCollect);
            
                unit.Data.SetDestination(rc.CollectorData.Building);
                unit.Agent.SetDestination(unit.Data.Destination.GetPosition());
                unit.Data.SetHasJobToExecute(true); 
                return;
            }

            if (rc.CollectorData.HasResource)
            {
                rc.DeliverResource(rc.CollectorData.Building, rc.CollectorData.ResourceTypeToCollect);

                unit.unitService.CreateJobNeededEvent(JobType.VillagerJob, null, rc.CollectorData.Building, rc.CollectorData.ResourceTypeToCollect, null);

                unit.unitService.CompleteJob(job);
                unit.Data.ResetJobState();
            }
        }
    }
}