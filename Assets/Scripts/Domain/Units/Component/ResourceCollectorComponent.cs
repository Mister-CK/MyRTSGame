using Buildings.Model;
using Enums;
using Interface;
using MyRTSGame.Model;
using Units.Model.Data;
using Units.Model.JobExecutors;

namespace Units.Model.Component
{
    public class ResourceCollectorComponent : UnitComponent
    {
        protected override JobType DefaultJobType => JobType.CollectResourceJob;
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
                unitService.CreateJobNeededEvent(JobType.LookForBuildingJob, CollectorData.Building, null, null, CollectorData.Building.GetOccupantType());
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
            unitService.RemoveResourceFromDestination(destination, resourceType, 1);
        }

        public void DeliverResource(IDestination destination, ResourceType resourceType)
        {
            CollectorData.SetHasResource(false);
            unitService.AddResourceToDestination(destination, resourceType, 1);
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