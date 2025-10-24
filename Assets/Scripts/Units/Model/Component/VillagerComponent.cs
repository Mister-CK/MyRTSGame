using Enums;
using Interface;
using MyRTSGame.Model;
using Units.Model.Data;

namespace Units.Model.Component
{
    public class VillagerComponent : UnitComponent
    {
        protected override JobType DefaultJobType => JobType.VillagerJob;
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

        protected override void ExecuteJob()
        {
            base.ExecuteJob();
            if (Data.CurrentJob is not VillagerJob villagerJob) return;
            
            if (!VillagerData.GetHasResource())
            {
                TakeResource(villagerJob.Origin, villagerJob.ResourceType);
                Data.SetDestination(villagerJob.Destination);
                Data.SetHasJobToExecute(true);
                Agent.SetDestination(Data.Destination.GetPosition());
                return;
            }
            DeliverResource(villagerJob.Destination, villagerJob.ResourceType);
            unitService.CompleteJob(Data.CurrentJob);
            Data.SetHasDestination(false);
            Data.SetCurrentJob(null);
            Data.SetDestination(null);
        }

        private void TakeResource(IDestination destination, ResourceType resourceType)
        {
            VillagerData.SetHasResource(true);
            unitService.RemoveResourceFromDestination(destination, resourceType, 1);
        }

        private void DeliverResource(IDestination destination, ResourceType resourceType)
        {
            VillagerData.SetHasResource(false);
            unitService.AddResourceToDestination(destination, resourceType, 1);
        }
    }
}
