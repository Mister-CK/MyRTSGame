using System;
using UnityEngine;

namespace MyRTSGame.Model
{
    public class JobController :  MonoBehaviour
    {
        [SerializeField] private GameEvent onCreateJobsForWarehouse;
        [SerializeField] private GameEvent onNewBuilderJobNeeded;
        [SerializeField] private GameEvent onNewVillagerJobNeeded;
        [SerializeField] private GameEvent onDeleteEvent;
        
        [SerializeField] private BuilderJobQueue builderJobQueue;
        [SerializeField] private VillagerJobQueue villagerJobQueue;

        private static BuildingList BuildingList => BuildingList.Instance;
        
        private void OnEnable()
        {
            onNewBuilderJobNeeded.RegisterListener(HandleNewJobNeeded);
            onNewVillagerJobNeeded.RegisterListener(HandleNewVillagerJobNeeded);
            onCreateJobsForWarehouse.RegisterListener(CreateJobsForBuilding);
            onDeleteEvent.RegisterListener(HandleDeleteBuildingEvent);
        }

        private void OnDisable()
        {
            onNewBuilderJobNeeded.UnregisterListener(HandleNewJobNeeded);
            onNewVillagerJobNeeded.UnregisterListener(HandleNewVillagerJobNeeded);
            onCreateJobsForWarehouse.UnregisterListener(CreateJobsForBuilding);
            onDeleteEvent.UnregisterListener(HandleDeleteBuildingEvent);
        }
        
        private static Building FindDestinationForJob(VillagerJob villagerJob)
        {
            var buildings = BuildingList.GetBuildings();
            Building destination = null;
            Building warehouse = null;
            var resourceType = villagerJob.ResourceType;

            foreach (var building in buildings)
            {
                if (building.BuildingType == BuildingType.Warehouse)
                {
                    warehouse = building;
                    continue;
                }

                var inputTypes = building.InputTypes;
                var inventory = building.GetInventory();
                var resourcesInJobForBuilding = building.ResourcesInJobForBuilding;

                if (Array.IndexOf(inputTypes, resourceType) == -1) continue;
                var resourceInInventory = Array.Find(inventory, res => res.ResourceType == resourceType);
                var resourceInJobForBuilding = Array.Find(resourcesInJobForBuilding, res => res.ResourceType == resourceType);
                if (resourceInInventory != null && resourceInInventory.Quantity + resourceInJobForBuilding.Quantity >= building.GetCapacity()) continue;
                destination = building;
                
                foreach(var res in destination.ResourcesInJobForBuilding)
                {
                    if (res.ResourceType == resourceType) res.Quantity++;
                }
                break;
            }

            // If no suitable building is found, set destination to Warehouse
            if (destination == null)
            {
                destination = warehouse;
                if (destination == null) throw new Exception("No Destination found");
            }

            return destination;
        }
        
        private void CreateJobsForBuilding(IGameEventArgs args)
        {
            if (args is not BuildingEventArgs buildingEventArgs) return;

            var building = buildingEventArgs.Building;
            foreach (var resource in building.GetInventory())
            {
                if (resource.Quantity <= 0)
                {
                    continue;
                }

                var resourceCount = resource.Quantity;
                while (resourceCount > 0)
                {
                    var job = new VillagerJob { Origin = building, ResourceType = resource.ResourceType };
                    var destination = FindDestinationForJob(job);

                    if (destination == null || destination == building)
                    {
                        break;
                    }

                    job.Destination = destination;
                    villagerJobQueue.AddJob(job);
                    resourceCount--;
                }
            }
        }
        
        private void HandleNewJobNeeded(IGameEventArgs args)
        {
            if (args is not BuildingEventArgs buildingEventArgs) return;
            
            var building = buildingEventArgs.Building;
            var builderJob = new BuilderJob() { Destination = building };
            builderJobQueue.AddJob(builderJob);
        }
        
        private void HandleNewVillagerJobNeeded(IGameEventArgs args)
        {
            if (args is not BuildingResourceTypeEventArgs buildingResourceTypeEventArgs) return;
            
            var villagerJob = new VillagerJob { Origin = buildingResourceTypeEventArgs.Building, ResourceType = buildingResourceTypeEventArgs.ResourceType };
            villagerJob.Destination = FindDestinationForJob(villagerJob);
            villagerJobQueue.AddJob(villagerJob);
        }
        
        private void HandleDeleteBuildingEvent(IGameEventArgs args)
        {
            if (args is not BuildingEventArgs buildingEventArgs) return;
            
            var building = buildingEventArgs.Building;
            builderJobQueue.RemoveJobsForBuilding(building);
            villagerJobQueue.RemoveJobsForBuilding(building);
        }
    }
}