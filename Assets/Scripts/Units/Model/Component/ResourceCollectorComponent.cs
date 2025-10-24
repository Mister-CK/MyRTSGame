using Buildings.Model;
using Enums;
using Interface;
using MyRTSGame.Model;
using Units.Model.Data; 
using MyRTSGame.Model.ResourceSystem.Model.NaturalResources;

namespace Units.Model.Component
{
    public class ResourceCollectorComponent : UnitComponent
    {
        protected override JobType DefaultJobType => JobType.CollectResourceJob;
        
        protected override UnitData CreateUnitData()
        {
            return new ResourceCollectorData();
        }

        public ResourceCollectorData CollectorData => (ResourceCollectorData)Data;
        
        protected override void ExecuteJob()
        {
            base.ExecuteJob();
            if (Data.CurrentJob == null) return;
            
            if (Data.Destination != CollectorData.Building)
            {
                if (Data.CurrentJob is PlantResourceJob)
                {
                    unitService.CreatePlantResourceEvent(Data.CurrentJob);
                    unitService.CompleteJob(Data.CurrentJob);
                }
                
                if (Data.CurrentJob is CollectResourceJob collectResourceJob)
                {
                    TakeResource(collectResourceJob.Destination, CollectorData.ResourceTypeToCollect);
                    
                    // Cleanup (Must use the Destination in Data)
                    // if (collectResourceJob.Destination is Wheat wheat) wheat.GetTerrain().SetHasResource(false);
                    // if (collectResourceJob.Destination is Grapes grapes) grapes.GetTerrain().SetHasResource(false);
                }
                
                Data.SetDestination(CollectorData.Building);
                Agent.SetDestination(Data.Destination.GetPosition());
                Data.SetHasJobToExecute(true); 
                
                return;
            }

            if (Data.CurrentJob is CollectResourceJob collectResourceJob2)
            {
                DeliverResource(CollectorData.Building, CollectorData.ResourceTypeToCollect);
                unitService.CreateJobNeededEvent(
                    JobType.VillagerJob, null, CollectorData.Building, CollectorData.ResourceTypeToCollect, null
                );
            }

            unitService.CompleteJob(Data.CurrentJob);
            Data.ResetJobState();
        }
        
        protected override void HandlePreDeletionCleanup()
        {
            unitService.CreateJobNeededEvent(JobType.LookForBuildingJob, CollectorData.Building, null, null, CollectorData.Building.GetOccupantType());
        }
        
        protected override void HandleLookingForBuildingJob(LookingForBuildingJob job)
        {
            if (job.Destination is not Building building) return;
            CollectorData.SetBuilding(building); 
            CollectorData.SetResourceTypeToCollect(building.OutputTypesWhenCompleted[0]);
        }
        
        private void TakeResource(IDestination destination, ResourceType resourceType)
        {
            CollectorData.SetHasResource(true);
            unitService.RemoveResourceFromDestination(destination, resourceType, 1);
        }

        private void DeliverResource(IDestination destination, ResourceType resourceType)
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