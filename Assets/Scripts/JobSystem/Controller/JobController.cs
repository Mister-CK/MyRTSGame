using System;
using UnityEngine;

namespace MyRTSGame.Model
{
    public class JobController :  MonoBehaviour
    {
        [SerializeField] private GameEvent onCreateJobsForWarehouse;
        [SerializeField] private GameEvent onDeleteVillagerJobsEvent;
        [SerializeField] private GameEvent onDeleteBuilderJobsEvent;
        [SerializeField] private GameEvent onVillagerJobDeleted;
        [SerializeField] private GameEvent onBuilderJobDeleted;
        [SerializeField] private GameEvent onNewJobCreated;
        [SerializeField] private GameEvent onNewJobNeeded;
        [SerializeField] private GameEvent onAssignJob;
        [SerializeField] private GameEvent onRequestUnitJob;
        [SerializeField] private GameEvent onJobRequestDenied;
        [SerializeField] private BuilderJobQueue builderJobQueue;
        [SerializeField] private VillagerJobQueue villagerJobQueue;
        [SerializeField] private ConsumptionJobQueue consumptionJobQueue;

        private static BuildingList BuildingList => BuildingList.Instance;
        
        private void OnEnable()
        {
            onNewJobNeeded.RegisterListener(HandleNewJobNeeded);
            onCreateJobsForWarehouse.RegisterListener(CreateJobsForBuilding);
            onDeleteVillagerJobsEvent.RegisterListener(HandleDeleteVillagerJobsEvent);
            onDeleteBuilderJobsEvent.RegisterListener(HandleDeleteBuilderJobsEvent);
            onRequestUnitJob.RegisterListener(HandleUnitJobRequest);
        }

        private void OnDisable()
        {
            onNewJobNeeded.UnregisterListener(HandleNewJobNeeded);
            onCreateJobsForWarehouse.UnregisterListener(CreateJobsForBuilding);
            onDeleteVillagerJobsEvent.UnregisterListener(HandleDeleteVillagerJobsEvent);
            onDeleteBuilderJobsEvent.UnregisterListener(HandleDeleteBuilderJobsEvent);
            onRequestUnitJob.UnregisterListener(HandleUnitJobRequest);
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
                    onNewJobCreated.Raise(new JobEventArgs(job));
                    villagerJobQueue.AddJob(job);
                    resourceCount--;
                }
            }
        }
        
        private void HandleNewJobNeeded(IGameEventArgs args)
        {
            if (args is not CreateNewJobEventArgs createNewJobEventArgs) return;
            switch (createNewJobEventArgs.JobType) {
                case JobType.BuilderJob:
                    CreateBuilderJob(createNewJobEventArgs);
                    return;
                case JobType.VillagerJob:
                    CreateVillagerJob(createNewJobEventArgs);
                    return;
                case JobType.ConsumptionJob:
                    CreateConsumptionJob(createNewJobEventArgs);
                    return;
                default:
                    throw new ArgumentException("invalid JobType provided to HandleNewBuilderJobNeeded");
            }

        }
        
        private void CreateBuilderJob(CreateNewJobEventArgs createNewJobEventArgs)
        {
            var builderJob = new BuilderJob() { Destination = createNewJobEventArgs.Destination };
            builderJobQueue.AddJob(builderJob);
            onNewJobCreated.Raise(new JobEventArgs(builderJob));
        }
        
        private void CreateVillagerJob(CreateNewJobEventArgs createNewJobEventArgs)
        {
            var villagerJob = new VillagerJob { Origin = createNewJobEventArgs.Origin, ResourceType = createNewJobEventArgs.ResourceType.GetValueOrDefault()};
            villagerJob.Destination = FindDestinationForJob(villagerJob);
            villagerJob.SetInProgress(false);
            villagerJobQueue.AddJob(villagerJob);
            onNewJobCreated.Raise(new JobEventArgs(villagerJob));
        }
        
        private void CreateConsumptionJob(CreateNewJobEventArgs createNewJobEventArgs)
        {
            var consumptionJob = new ConsumptionJob() { Destination = createNewJobEventArgs.Destination, ResourceType = createNewJobEventArgs.ResourceType.GetValueOrDefault()};
            consumptionJobQueue.AddJob(consumptionJob);
            onNewJobCreated.Raise(new JobEventArgs(consumptionJob));
        }

        private void HandleDeleteVillagerJobsEvent(IGameEventArgs args)
        {   
            if (args is not VillagerJobListEventArgs jobListEventArgs) return;

            foreach (var villagerJob in jobListEventArgs.VillagerJobs)
            {
                villagerJobQueue.RemoveJob(villagerJob);
                Debug.Log("Delete Villager Job in progress");
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
            if (args is not UnitWithJobTypeEventArgs unitWithJobTypeEventArgs) return;
            switch (unitWithJobTypeEventArgs.JobType)
            {
                case JobType.BuilderJob:
                    var builderJob = builderJobQueue.GetNextJob();
                    if (builderJob == null)           
                    {
                        onJobRequestDenied.Raise(new UnitEventArgs(unitWithJobTypeEventArgs.Unit));
                        return;
                    }
                    builderJob.Builder = unitWithJobTypeEventArgs.Unit as Builder;
                    builderJob.SetInProgress(true);
                    onAssignJob.Raise(new UnitWithJobEventArgs(unitWithJobTypeEventArgs.Unit, builderJob));
                    return;
                case JobType.VillagerJob:
                    var villagerJob = villagerJobQueue.GetNextJob();
                    if (villagerJob == null)          
                    {
                        onJobRequestDenied.Raise(new UnitEventArgs(unitWithJobTypeEventArgs.Unit));
                        return;
                    }
                    villagerJob.Villager = unitWithJobTypeEventArgs.Unit as Villager;
                    villagerJob.SetInProgress(true);
                    onAssignJob.Raise(new UnitWithJobEventArgs(unitWithJobTypeEventArgs.Unit, villagerJob));
                    return;
                case JobType.ConsumptionJob:
                    var consumptionJob = consumptionJobQueue.GetNextJob();
                    if (consumptionJob == null)
                    {
                        onJobRequestDenied.Raise(new UnitEventArgs(unitWithJobTypeEventArgs.Unit));
                        return;
                    }
                    consumptionJob.Unit = unitWithJobTypeEventArgs.Unit;
                    consumptionJob.SetInProgress(true);
                    onAssignJob.Raise(new UnitWithJobEventArgs(unitWithJobTypeEventArgs.Unit, consumptionJob));
                    return;
                default:
                    throw new ArgumentException("JobType not recognized in HandleUnitJobRequest");
            }
        }
        
    }
}