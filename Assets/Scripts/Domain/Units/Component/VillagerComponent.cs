using Enums;
using Interface;
using Domain;
using Domain.Model;
using System;
using Domain.Units.Data;
using Domain.Units.JobExecutors;

namespace Domain.Units.Component
{
    public class VillagerComponent : UnitComponent
    {
        protected override JobType DefaultJobType => JobType.VillagerJob;
        public Action<IDestination, ResourceType, int> OnAddResourceToDestination { get; set; }

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
            OnRemoveResourceFromDestination?.Invoke(destination, resourceType, 1);
        }

        public void DeliverResource(IDestination destination, ResourceType resourceType)
        {
            VillagerData.SetHasResource(false);
            OnAddResourceToDestination?.Invoke(destination, resourceType, 1);
        }
    }
}
