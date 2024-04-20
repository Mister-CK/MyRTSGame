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
        [SerializeField] private GameEvent onAssignJob;
        [SerializeField] private GameEvent onVillagerJobDeleted;
        [SerializeField] private GameEvent onDeleteBuilderJobsEvent;
        [SerializeField] private GameEvent onBuilderJobDeleted;
        [SerializeField] private GameEvent onNewBuilderJobCreated;
        [SerializeField] private GameEvent onRequestUnitJob;
        [SerializeField] private GameEvent onRequestConsumptionJob;
        [SerializeField] private GameEvent onNewConsumptionJobNeeded;
        [SerializeField] private GameEvent onNewConsumptionJobCreated;
        [SerializeField] private GameEvent onJobRequestDenied;
        
        [SerializeField] private BuilderJobQueue builderJobQueue;
        [SerializeField] private VillagerJobQueue villagerJobQueue;
        [SerializeField] private ConsumptionJobQueue consumptionJobQueue;

        private static BuildingList BuildingList => BuildingList.Instance;
        
        private void OnEnable()
        {
            onNewBuilderJobNeeded.RegisterListener(HandleNewBuilderJobNeeded);
            onNewVillagerJobNeeded.RegisterListener(HandleNewVillagerJobNeeded);
            onNewConsumptionJobNeeded.RegisterListener(HandleNewConsumptionJobNeeded);
            onCreateJobsForWarehouse.RegisterListener(CreateJobsForBuilding);
            onDeleteVillagerJobsEvent.RegisterListener(HandleDeleteVillagerJobsEvent);
            onDeleteBuilderJobsEvent.RegisterListener(HandleDeleteBuilderJobsEvent);
            onRequestUnitJob.RegisterListener(HandleUnitJobRequest);
            onRequestConsumptionJob.RegisterListener(HandleConsumptionJobRequest);
        }

        private void OnDisable()
        {
            onNewBuilderJobNeeded.UnregisterListener(HandleNewBuilderJobNeeded);
            onNewVillagerJobNeeded.UnregisterListener(HandleNewVillagerJobNeeded);
            onNewConsumptionJobNeeded.UnregisterListener(HandleNewConsumptionJobNeeded);
            onCreateJobsForWarehouse.UnregisterListener(CreateJobsForBuilding);
            onDeleteVillagerJobsEvent.UnregisterListener(HandleDeleteVillagerJobsEvent);
            onDeleteBuilderJobsEvent.UnregisterListener(HandleDeleteBuilderJobsEvent);
            onRequestUnitJob.UnregisterListener(HandleUnitJobRequest);
            onRequestConsumptionJob.UnregisterListener(HandleConsumptionJobRequest);
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
            
            var builderJob = new BuilderJob() { Destination = buildingEventArgs.Building };
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
        
        private void HandleNewConsumptionJobNeeded(IGameEventArgs args)
        {
            if (args is not BuildingResourceTypeEventArgs buildingResourceTypeEventArgs) return;
            
            var consumptionJob = new ConsumptionJob() { Destination = buildingResourceTypeEventArgs.Building, ResourceType = buildingResourceTypeEventArgs.ResourceType};
            consumptionJobQueue.AddJob(consumptionJob);
            onNewConsumptionJobCreated.Raise(new ConsumptionJobEventArgs(consumptionJob));
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

        private void HandleUnitJobRequest(IGameEventArgs args)
        {
            if (args is not UnitEventArgs unitEventArgs) return;
            if (unitEventArgs.Unit is Builder builder)
            {
                var builderJob = builderJobQueue.GetNextJob();
                if (builderJob == null)           
                {
                    onJobRequestDenied.Raise(new UnitEventArgs(unitEventArgs.Unit));
                    return;
                }
                builderJob.Builder = builder;
                onAssignJob.Raise(new UnitWithJobEventArgs(builder, builderJob));
                return;
            }
            if (unitEventArgs.Unit is Villager villager)
            {
                var villagerJob = villagerJobQueue.GetNextJob();
                if (villagerJob == null)          
                {
                    onJobRequestDenied.Raise(new UnitEventArgs(unitEventArgs.Unit));
                    return;
                }
                villagerJob.Villager = villager;
                onAssignJob.Raise(new UnitWithJobEventArgs(villager, villagerJob));
                return;
            }
        }
        
        private void HandleConsumptionJobRequest(IGameEventArgs args)
        {
            if (args is not UnitEventArgs unitEventArgs) return;
            
            var consumptionJob = consumptionJobQueue.GetNextJob();
            if (consumptionJob == null)
            {
                onJobRequestDenied.Raise(new UnitEventArgs(unitEventArgs.Unit));
                return;
            }
            consumptionJob.Unit = unitEventArgs.Unit;
            onAssignJob.Raise(new UnitWithJobEventArgs(unitEventArgs.Unit, consumptionJob));
        }
    }
}