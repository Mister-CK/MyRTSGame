using Enums;
using Interface;
using MyRTSGame.Model;
using Units.Model.Data;
using Units.Model.JobExecutors;

namespace Units.Model.Component
{
    public class VillagerComponent : UnitComponent
    {
        protected override JobType DefaultJobType => JobType.VillagerJob;
        
        static VillagerComponent()
        {
            JobExecutorsMap.Add(typeof(VillagerJob), new VillagerJobExecutor());
        }
        
        protected override void HandleJobAssignment(Job job)
        {
            if (job is VillagerJob villagerJob) Data.SetDestination(villagerJob.Origin); 
        }
        protected override UnitData CreateUnitData()
        {
            return new VillagerData();
        }

        protected override void HandleUnAssignCleanup(DestinationType destinationType)
        {
            if (destinationType == DestinationType.Origin && VillagerData.GetHasResource())
            {
                return;
            }
            VillagerData.SetHasResource(false);
        }

        public VillagerData VillagerData => (VillagerData)Data;

        public void TakeResource(IDestination destination, ResourceType resourceType)
        {
            VillagerData.SetHasResource(true);
            unitService.RemoveResourceFromDestination(destination, resourceType, 1);
        }

        public void DeliverResource(IDestination destination, ResourceType resourceType)
        {
            VillagerData.SetHasResource(false);
            unitService.AddResourceToDestination(destination, resourceType, 1);
        }
    }
}
