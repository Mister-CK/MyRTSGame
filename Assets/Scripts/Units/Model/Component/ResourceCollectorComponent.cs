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
        protected override UnitData CreateUnitData()
        {
            return new ResourceCollectorData();
        }

        public ResourceCollectorData CollectorData => (ResourceCollectorData)Data;

        // --- CORE JOB EXECUTION LOGIC ---
        
        protected override void ExecuteJob()
        {
            // 1. Run base logic first.
            // This handles ConsumptionJob (finishes, resets state) AND
            // LookingForBuildingJob (calls HandleLookingForBuildingJob then finishes).
            base.ExecuteJob();
            
            // If the base class finished a job (like ConsumptionJob or LookingForBuildingJob), 
            // CurrentJob is null, and we are done for this execution cycle.
            if (Data.CurrentJob == null) return;
            
            
            // --- RESOURCE COLLECTOR HOP 1: ARRVIVED AT RESOURCE/PLANTING SITE ---
            // If the unit's current destination is NOT the building (i.e., it's a resource or planting site).
            if (Data.Destination != CollectorData.GetBuilding())
            {
                if (Data.CurrentJob is PlantResourceJob)
                {
                    unitService.CreatePlantResourceEvent(Data.CurrentJob);
                    
                    // The job is COMPLETED once planted.
                    unitService.CompleteJob(Data.CurrentJob);
                }
                
                if (Data.CurrentJob is CollectResourceJob collectResourceJob)
                {
                    // Action 1: Take Resource
                    TakeResource(collectResourceJob.Destination, CollectorData.ResourceTypeToCollect);
                    
                    // Cleanup (Must use the Destination in Data)
                    // if (collectResourceJob.Destination is Wheat wheat) wheat.GetTerrain().SetHasResource(false);
                    // if (collectResourceJob.Destination is Grapes grapes) grapes.GetTerrain().SetHasResource(false);
                }
                
                // CRITICAL 2-HOP LOGIC: Set the NEW destination to the building and initiate the second hop.
                Data.SetDestination(CollectorData.GetBuilding());
                Agent.SetDestination(Data.Destination.GetPosition());
                Data.SetHasJobToExecute(true); // Forces the Update loop to move the Agent.
                
                // DO NOT reset CurrentJob or HasDestination here. We are only halfway done.
                return;
            }

            
            // --- RESOURCE COLLECTOR HOP 2: ARRVIVED AT BUILDING (FINAL STEP) ---
            // If Data.Destination IS the building.
            
            if (Data.CurrentJob is CollectResourceJob collectResourceJob2)
            {
                // Action 1: Deliver Resource
                DeliverResource(CollectorData.GetBuilding(), CollectorData.ResourceTypeToCollect);
                
                // Action 2: Trigger follow-up job (Villager)
                unitService.CreateJobNeededEvent(
                    JobType.VillagerJob, null, CollectorData.GetBuilding(), CollectorData.ResourceTypeToCollect, null
                );
            }

            // FINAL STEP: The job is truly complete, regardless of Collect or Plant.
            // This resets the unit's state and prepares it to request a new job.
            unitService.CompleteJob(Data.CurrentJob);
            Data.ResetJobState(); // Sets HasDestination = false, CurrentJob = null.
        }
        
        protected override void HandleLookingForBuildingJob(LookingForBuildingJob job)
        {
            if (job.Destination is Building building)
            {
                CollectorData.SetBuilding(building); 
                CollectorData.SetResourceTypeToCollect(building.OutputTypesWhenCompleted[0]);
            }
        }
        
        // --- RESOURCE INTERACTION METHODS (Service Calls) ---

        private void TakeResource(IDestination destination, ResourceType resourceType)
        {
            CollectorData.SetHasResource(true); // Data change via POCO method
            unitService.RemoveResourceFromDestination(destination, resourceType, 1);
        }

        private void DeliverResource(IDestination destination, ResourceType resourceType)
        {
            CollectorData.SetHasResource(false); // Data change via POCO method
            unitService.AddResourceToDestination(destination, resourceType, 1);
        }
        
        // --- EXTERNAL COMMANDS (Called by UnitService on Building Deletion) ---
        
        public void BuildingDeleted()
        {
            // DATA UPDATE: Reset all collector-specific state
            Data.ResetJobState();
            CollectorData.SetHasResource(false);
            CollectorData.SetBuilding(null);
            Data.SetIsLookingForBuilding(true); // Force unit to find a new building
            
            // ENGINE EXECUTION: Stop the NavMeshAgent
            Agent.SetDestination(Agent.transform.position);
        }
    }
}