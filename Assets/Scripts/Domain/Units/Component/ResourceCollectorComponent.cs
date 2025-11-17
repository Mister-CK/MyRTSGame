using Buildings.Model;
using Enums;
using Interface;
using JetBrains.Annotations;
using Domain;
using Domain.Model;
using System;
using Domain.Units.Data;
using Domain.Units.JobExecutors;

namespace Domain.Units.Component
{
    public class ResourceCollectorComponent : UnitComponent
    {
        protected override JobType DefaultJobType => JobType.CollectResourceJob;
        public Action<IDestination, ResourceType, int> OnAddResourceToDestination { get; set; }
        [CanBeNull]
        public Action<JobType, Building, Building, ResourceType?, UnitType?> OnCreateJobNeededEvent { get; set; }
        public Action<Job> OnCreatePlantResourceEvent { get; set; }
        static ResourceCollectorComponent()
        {
            JobExecutorsMap.Add(typeof(CollectResourceJob), new CollectResourceExecutor());
            JobExecutorsMap.Add(typeof(PlantResourceJob), new PlantResourceExecutor());
        }
        
        protected override UnitData CreateUnitData()
        {
            return new ResourceCollectorData();
        }

        public ResourceCollectorData CollectorData => (ResourceCollectorData)Data;
        
        protected override void HandlePreDeletionCleanup()
        {
            if (CollectorData.Building != null)
            {
                OnCreateJobNeededEvent?.Invoke(JobType.LookForBuildingJob, CollectorData.Building, null, null, CollectorData.Building.GetOccupantType());
            }
        }
        
        public override void HandleLookingForBuildingJob(LookingForBuildingJob job)
        {
            if (job.Destination is not Building building) return;
            CollectorData.SetBuilding(building); 
            CollectorData.SetResourceTypeToCollect(building.OutputTypesWhenCompleted[0]);
        }
        
        public void TakeResource(IDestination destination, ResourceType resourceType)
        {
            CollectorData.SetHasResource(true);
            OnRemoveResourceFromDestination?.Invoke(destination, resourceType, 1);
        }

        public void DeliverResource(IDestination destination, ResourceType resourceType)
        {
            CollectorData.SetHasResource(false);
            OnAddResourceToDestination?.Invoke(destination, resourceType, 1);
        }

        public void BuildingDeleted()
        {
            Data.ResetJobState();
            CollectorData.SetHasResource(false);
            CollectorData.SetBuilding(null);
            Data.SetIsLookingForBuilding(true);
            
            Agent.SetDestination(Agent.transform.position);
        }
    }
}