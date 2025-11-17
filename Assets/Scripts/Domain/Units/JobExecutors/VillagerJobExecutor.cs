using Buildings.Model;
using Interface;
using MyRTSGame.Model;
using Units.Model.Component;
using UnityEngine;

namespace Units.Model.JobExecutors
{
    public class VillagerJobExecutor: IJobExecutor
    {
        public void Execute(UnitComponent unitComponent, Job job)
        {
            var villagerComponent = (VillagerComponent)unitComponent;
            var villagerJob = (VillagerJob)job;
            if (!villagerComponent.VillagerData.GetHasResource())
            {
                villagerComponent.TakeResource(villagerJob.Origin, villagerJob.ResourceType);
                villagerComponent.Data.SetDestination(villagerJob.Destination);
                villagerComponent.Data.SetHasJobToExecute(true);
                villagerComponent.Agent.SetDestination(villagerComponent.Data.Destination.GetPosition());
                return;
            }
            villagerComponent.DeliverResource(villagerJob.Destination, villagerJob.ResourceType);
            villagerComponent.OnJobCompleted?.Invoke(villagerComponent.Data.CurrentJob);
            villagerComponent.Data.SetHasDestination(false);
            villagerComponent.Data.SetCurrentJob(null);
            villagerComponent.Data.SetDestination(null);
        }
    }
}
