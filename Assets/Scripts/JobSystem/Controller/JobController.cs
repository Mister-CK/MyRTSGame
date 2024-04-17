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
        [SerializeField] private GameEvent onDeleteBuilderJobsEvent;
        [SerializeField] private GameEvent onBuilderJobAssigned;
        [SerializeField] private GameEvent onBuilderJobDeleted;
        [SerializeField] private GameEvent onRequestBuilderJob;
        [SerializeField] private GameEvent onNewBuilderJobCreated;

        [SerializeField] private BuilderJobQueue builderJobQueue;
        [SerializeField] private VillagerJobQueue villagerJobQueue;

        private static BuildingList BuildingList => BuildingList.Instance;
        
        private void OnEnable()
        {
            onNewBuilderJobNeeded.RegisterListener(HandleNewBuilderJobNeeded);
            onNewVillagerJobNeeded.RegisterListener(HandleNewVillagerJobNeeded);
            onCreateJobsForWarehouse.RegisterListener(CreateJobsForBuilding);
            onDeleteVillagerJobsEvent.RegisterListener(HandleDeleteVillagerJobsEvent);
            onDeleteBuilderJobsEvent.RegisterListener(HandleDeleteBuilderJobsEvent);
            onRequestVillagerJob.RegisterListener(HandleRequestVillagerJob);
            onRequestBuilderJob.RegisterListener(HandleRequestBuilderJob);
        }

        private void OnDisable()
        {
            onNewBuilderJobNeeded.UnregisterListener(HandleNewBuilderJobNeeded);
            onNewVillagerJobNeeded.UnregisterListener(HandleNewVillagerJobNeeded);
            onCreateJobsForWarehouse.UnregisterListener(CreateJobsForBuilding);
            onDeleteVillagerJobsEvent.UnregisterListener(HandleDeleteVillagerJobsEvent);
            onDeleteBuilderJobsEvent.UnregisterListener(HandleDeleteBuilderJobsEvent);
            onRequestVillagerJob.UnregisterListener(HandleRequestVillagerJob);
            onRequestBuilderJob.UnregisterListener(HandleRequestBuilderJob);

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

                if (Array.IndexOf(inputTypes, resourceType) == -1) continue;
                if (inventory[resourceType].Current + inventory[resourceType].Incoming >= building.GetCapacity()) continue;
                destination = building;
                
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
            onNewBuilderJobCreated.Raise(new BuilderJobEventArgs(builderJob));
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
        
        private void HandleDeleteBuilderJobsEvent(IGameEventArgs args)
        {
            if (args is not BuilderJobListEventArgs jobListEventArgs) return;

            foreach (var builderJob in jobListEventArgs.BuilderJobs)
            {
                builderJobQueue.RemoveJob(builderJob);
                onBuilderJobDeleted.Raise(new BuilderWithJobEventArgs(builderJob.Builder, builderJob));
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
        
        private void HandleRequestBuilderJob(IGameEventArgs args)
        {
            if (args is not BuilderEventArgs builderEventArgs) return;

            var builderJob = builderJobQueue.GetNextJob();
            if (builderJob == null) return;
            builderJob.Builder = builderEventArgs.Builder;
            onBuilderJobAssigned.Raise(new BuilderWithJobEventArgs(builderEventArgs.Builder, builderJob));
        }
    }
}