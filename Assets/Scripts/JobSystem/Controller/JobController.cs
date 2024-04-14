using System;
using UnityEngine;

namespace MyRTSGame.Model
{
    public class JobController :  MonoBehaviour
    {
        [SerializeField] private GameEvent onCreateJobsForWarehouse;
        [SerializeField] private GameEvent onNewBuilderJobNeeded;
        [SerializeField] private GameEvent onNewVillagerJobNeeded;
        [SerializeField] private GameEvent onNewVillagerJobCreated;
        [SerializeField] private GameEvent onDeleteVillagerJobsEvent;
        [SerializeField] private GameEvent onRequestVillagerJob;
        [SerializeField] private GameEvent onAssignVillagerJob;
        [SerializeField] private GameEvent onVillagerJobDeleted;
        
        [SerializeField] private BuilderJobQueue builderJobQueue;
        [SerializeField] private VillagerJobQueue villagerJobQueue;

        private static BuildingList BuildingList => BuildingList.Instance;
        
        private void OnEnable()
        {
            onNewBuilderJobNeeded.RegisterListener(HandleNewBuilderJobNeeded);
            onNewVillagerJobNeeded.RegisterListener(HandleNewVillagerJobNeeded);
            onCreateJobsForWarehouse.RegisterListener(CreateJobsForBuilding);
            onDeleteVillagerJobsEvent.RegisterListener(HandleDeleteVillagerJobsEvent);
            onRequestVillagerJob.RegisterListener(HandleRequestVillagerJob);
        }

        private void OnDisable()
        {
            onNewBuilderJobNeeded.UnregisterListener(HandleNewBuilderJobNeeded);
            onNewVillagerJobNeeded.UnregisterListener(HandleNewVillagerJobNeeded);
            onCreateJobsForWarehouse.UnregisterListener(CreateJobsForBuilding);
            onDeleteVillagerJobsEvent.UnregisterListener(HandleDeleteVillagerJobsEvent);
            onRequestVillagerJob.UnregisterListener(HandleRequestVillagerJob);
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
                var resourceInJobForBuilding = Array.Find(resourcesInJobForBuilding, res => res.ResourceType == resourceType);
                if (inventory[resourceType].Current + resourceInJobForBuilding.Quantity >= building.GetCapacity()) continue;
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
                if (resource.Value.Current <= 0)
                {
                    continue;
                }

                var resourceCount = resource.Value.Current;
                while (resourceCount > 0)
                {
                    var job = new VillagerJob { Origin = building, ResourceType = resource.Key};
                    var destination = FindDestinationForJob(job);
                    
                    if (destination == null || destination == building)
                    {
                        break;
                    }

                    job.Destination = destination;
                    job.SetInProgress(false);
                    onNewVillagerJobCreated.Raise(new VillagerJobEventArgs(job));
                    villagerJobQueue.AddJob(job);
                    resourceCount--;
                }
            }
        }
        
        private void HandleNewBuilderJobNeeded(IGameEventArgs args)
        {
            if (args is not BuildingEventArgs buildingEventArgs) return;
            
            var building = buildingEventArgs.Building;
            var builderJob = new BuilderJob() { Destination = building };
            builderJobQueue.AddJob(builderJob);
        }
        
        private void HandleNewVillagerJobNeeded(IGameEventArgs args)
        {
            if (args is not BuildingResourceTypeEventArgs buildingResourceTypeEventArgs) return;
            
            var villagerJob = new VillagerJob { Origin = buildingResourceTypeEventArgs.Building, ResourceType = buildingResourceTypeEventArgs.ResourceType};
            villagerJob.Destination = FindDestinationForJob(villagerJob);
            villagerJob.SetInProgress(false);
            villagerJobQueue.AddJob(villagerJob);
            onNewVillagerJobCreated.Raise(new VillagerJobEventArgs(villagerJob));
        }

        private void HandleDeleteVillagerJobsEvent(IGameEventArgs args)
        {   
            if (args is not VillagerJobListEventArgs jobListEventArgs) return;

            foreach (var villagerJob in jobListEventArgs.VillagerJobs)
            {
                villagerJobQueue.RemoveJob(villagerJob);
                if (!villagerJob.IsInProgress()) continue;
                onVillagerJobDeleted.Raise(new VillagerWithJobEventArgsAndDestinationtype(villagerJob.Villager,
                    villagerJob, jobListEventArgs.DestinationType));
            }
        }

        private void HandleRequestVillagerJob(IGameEventArgs args)
        {
            if (args is not VillagerEventArgs villagerEventArgs) return;

            var villagerJob = villagerJobQueue.GetNextJob();
            if (villagerJob == null) return;
            villagerJob.SetInProgress(true);
            villagerJob.Villager = villagerEventArgs.Villager;
            onAssignVillagerJob.Raise(new VillagerWithJobEventArgs(villagerEventArgs.Villager, villagerJob));
        }
    }
}