using System;
using System.Collections.Generic;
using System.Linq;
using MyRTSGame.Model.ResourceSystem.Model;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MyRTSGame.Model
{
    public class JobController :  MonoBehaviour
    {
        [SerializeField] private GameEvent onCreateJobsForWarehouse;
        [SerializeField] private GameEvent onDeleteVillagerJobsEvent;
        [SerializeField] private GameEvent onDeleteBuilderJobsEvent;
        [SerializeField] private GameEvent onUnitJobDeleted;
        [SerializeField] private GameEvent onNewJobCreated;
        [SerializeField] private GameEvent onNewJobNeeded;
        [SerializeField] private GameEvent onAssignJob;
        [SerializeField] private GameEvent onRequestUnitJob;
        [SerializeField] private GameEvent onJobRequestDenied;
        [SerializeField] private GameEvent onAddCollectResourceJobsEvent;
        [SerializeField] private GameEvent onDeleteUnitEvent;
        [SerializeField] private GameEvent onDeleteJobEvent;
        [SerializeField] private GameEvent onCompleteJobEvent;
        [SerializeField] private GameEvent onAbortJobEvent;
        
        [SerializeField] private BuilderJobQueue builderJobQueue;
        [SerializeField] private VillagerJobQueue villagerJobQueue;
        [SerializeField] private ConsumptionJobQueue consumptionJobQueue;
        [SerializeField] private LookingForBuildingJobQueue lookingForBuildingJobQueue;
        private static BuildingList BuildingList => BuildingList.Instance;
        
        private void OnEnable()
        {
            onNewJobNeeded.RegisterListener(HandleNewJobNeeded);
            onCreateJobsForWarehouse.RegisterListener(CreateJobsForBuilding);
            onDeleteVillagerJobsEvent.RegisterListener(HandleDeleteVillagerJobsEvent);
            onDeleteBuilderJobsEvent.RegisterListener(HandleDeleteBuilderJobsEvent);
            onRequestUnitJob.RegisterListener(HandleUnitJobRequest);
            onAddCollectResourceJobsEvent.RegisterListener(HandleOnAddResourceJobsEvent);
            onDeleteUnitEvent.RegisterListener(HandleDeleteUnitEvent);
            onCompleteJobEvent.RegisterListener(HandleCompleteJob);
            onAbortJobEvent.RegisterListener(HandleOnAbortJobEvent);
        }

        private void OnDisable()
        {
            onNewJobNeeded.UnregisterListener(HandleNewJobNeeded);
            onCreateJobsForWarehouse.UnregisterListener(CreateJobsForBuilding);
            onDeleteVillagerJobsEvent.UnregisterListener(HandleDeleteVillagerJobsEvent);
            onDeleteBuilderJobsEvent.UnregisterListener(HandleDeleteBuilderJobsEvent);
            onRequestUnitJob.UnregisterListener(HandleUnitJobRequest);
            onAddCollectResourceJobsEvent.UnregisterListener(HandleOnAddResourceJobsEvent);
            onDeleteUnitEvent.UnregisterListener(HandleDeleteUnitEvent);
            onCompleteJobEvent.UnregisterListener(HandleCompleteJob);
            onAbortJobEvent.UnregisterListener(HandleOnAbortJobEvent);

        }
        
        private static Building FindDestinationForJob(VillagerJob villagerJob)
        {
            var buildings = BuildingList.GetBuildings();
            Building destination = null;
            Building warehouse = null;
            var resourceType = villagerJob.ResourceType;

            foreach (var building in buildings)
            {
                if (building.GetBuildingType() == BuildingType.Warehouse)
                {
                    warehouse = building;
                    continue;
                }

                var inputTypes = building.InputTypes;
                var inventory = building.GetInventory();

                if (Array.IndexOf(inputTypes, resourceType) == -1) continue;
                if (inventory[resourceType].Current + inventory[resourceType].InJob >= building.GetCapacity()) continue;
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
                case JobType.LookForBuildingJob:
                    CreateLookingForBuildingJob(createNewJobEventArgs);
                    return;
                default:
                    throw new ArgumentException("invalid JobType provided to HandleNewBuilderJobNeeded");
            }
        }

        private void CreateLookingForBuildingJob(CreateNewJobEventArgs createNewJobEventArgs)
        {
            var lookingForBuildingJob = new LookingForBuildingJob() { Destination = createNewJobEventArgs.Destination, UnitType = createNewJobEventArgs.UnitType.GetValueOrDefault()};
            lookingForBuildingJobQueue.AddJob(lookingForBuildingJob);
            onNewJobCreated.Raise(new JobEventArgs(lookingForBuildingJob));
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
        
        private static List<ResourceBuilding> FindBuildingsWithResourceTypeWithinRange(NaturalResource naturalResource)
        {
            return BuildingList
                .GetBuildings()
                .OfType<ResourceBuilding>()
                .Where(resourceBuilding => Array.IndexOf(resourceBuilding.OutputTypesWhenCompleted, naturalResource.GetResourceType()) != -1)
                .Where(resourceBuilding => Vector3.Distance(resourceBuilding.transform.position, naturalResource.transform.position) <= 10)
                .ToList();
        }
        
        private void HandleOnAddResourceJobsEvent(IGameEventArgs args)
        {
            if (args is not NaturalResourceEventArgs naturalResourceEventArgs) return;
            
            var collectResourceJob = new CollectResourceJob()
            {
                Destination = naturalResourceEventArgs.NaturalResource,
                ResourceType = naturalResourceEventArgs.NaturalResource.GetResourceType()
            };

            var buildings = FindBuildingsWithResourceTypeWithinRange(naturalResourceEventArgs.NaturalResource);
            buildings.ForEach(building =>
            {
                building.AddCollectResourceJobToBuilding(collectResourceJob);
            });

            onNewJobCreated.Raise(new JobEventArgs(collectResourceJob));
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
                if (!villagerJob.IsInProgress()) continue;
                onUnitJobDeleted.Raise(new UnitWithJobEventArgsAndDestinationType(villagerJob.Unit, 
                    villagerJob, jobListEventArgs.DestinationType));
            }
        }
        
        private void HandleDeleteBuilderJobsEvent(IGameEventArgs args)
        {
            if (args is not BuilderJobListEventArgs jobListEventArgs) return;

            foreach (var builderJob in jobListEventArgs.BuilderJobs)
            {
                builderJobQueue.RemoveJob(builderJob);
                onUnitJobDeleted.Raise(new UnitWithJobEventArgsAndDestinationType(builderJob.Unit, builderJob, null));
            }
        }

        private void HandleUnitJobRequest(IGameEventArgs args)
        {
            if (args is not UnitWithJobTypeEventArgs unitWithJobTypeEventArgs) return;
            var newJob = unitWithJobTypeEventArgs.JobType switch
            {
                JobType.BuilderJob => builderJobQueue.GetNextJob(),
                JobType.VillagerJob => villagerJobQueue.GetNextJob(),
                JobType.ConsumptionJob => consumptionJobQueue.GetNextJob(),
                JobType.LookForBuildingJob => lookingForBuildingJobQueue.GetNextJobForUnitType(unitWithJobTypeEventArgs.Unit.GetUnitType()),
                JobType.CollectResourceJob => GetNextResourceCollectionJobForUnit(unitWithJobTypeEventArgs.Unit),
                _ => throw new ArgumentException("JobType not recognized in HandleUnitJobRequest")
            };
            if (newJob == null)           
            {
                onJobRequestDenied.Raise(new UnitEventArgs(unitWithJobTypeEventArgs.Unit));
                return;
            }
            newJob.Unit = unitWithJobTypeEventArgs.Unit;
            newJob.SetInProgress(true);
            if (newJob is VillagerJob villagerJob)
            {
                villagerJob.Origin.ModifyInventory(villagerJob.ResourceType, data => data.Outgoing += 1);
                if (villagerJob.Destination is Building building) building.ModifyInventory(villagerJob.ResourceType, data => data.Incoming += 1);
            }

            onAssignJob.Raise(new UnitWithJobEventArgs(unitWithJobTypeEventArgs.Unit, newJob));
        }
        private static Vector3 GetRandomPointToPlantTree(ResourceBuilding resourceBuilding)
        {
            var freeSpaceAroundPoint = 2f;
            var dist = resourceBuilding.GetMaxDistanceFromBuilding();
            var minDist = 4f; // Minimum distance from the building
            var maxAttempts = 1000; // Maximum number of attempts to find a suitable point
            var attempts = 0;

            var randomPoint = new Vector3(0, 0, 0);

            while (attempts < maxAttempts)
            {
                var randomRadius = Random.Range(minDist, dist);
                var randomAngle = Random.Range(0, 2 * Mathf.PI);

                var randomX = randomRadius * Mathf.Cos(randomAngle);
                var randomZ = randomRadius * Mathf.Sin(randomAngle);

                randomPoint = new Vector3(randomX + resourceBuilding.transform.localScale.x / 2, 0, randomZ + resourceBuilding.transform.localScale.z / 2) + resourceBuilding.transform.position;

                var colliders = Physics.OverlapSphere(randomPoint, freeSpaceAroundPoint);
                if (colliders.Length <= 1) break;

                attempts++;
            }

            if (attempts >= maxAttempts)
            {
                throw new Exception("Could not find a suitable point to plant tree");
            }

            return randomPoint;
        }

        private Job GetNextResourceCollectionJobForUnit(Unit unit)
        {
            if (unit is not ResourceCollector resourceCollector) return null;
            if (resourceCollector.GetBuilding() is not ResourceBuilding resourceBuilding) return null;
            Job job = resourceBuilding.GetCollectResourceJob(resourceCollector.GetResourceTypeToCollect()); 
            if (job != null) return job;

            return resourceCollector switch
            {
                LumberJack => CreatePlantResourceJob(resourceCollector),
                Farmer => CreatePlantWheatJob(resourceCollector),
                StoneMiner => null,
                _ => null,
            };
        }
        
        private Terrains.Model.Terrain FindAvailableTerrainWithinRadius(ResourceType resourceType, float radius, Vector3 position)
        {
            return Physics.OverlapSphere(position, radius)
                .Select(hitCollider => hitCollider.GetComponentInParent<Terrains.Model.Terrain>())
                .FirstOrDefault(terrain => terrain != null &&
                                           terrain.GetState() is Terrains.Model.TerrainStates.CompletedState && 
                                           !terrain.GetHasResource() &&
                                           terrain.GetResourceType() == resourceType);
        }
        
        private Job CreatePlantWheatJob(ResourceCollector resourceCollector)
        {
            var farmlandToPlantWheat = FindAvailableTerrainWithinRadius(resourceCollector.GetResourceTypeToCollect(), 10f, resourceCollector.GetBuilding().transform.position);
            if (farmlandToPlantWheat == null) return null; //no available farmland found;
            
            Job job = new PlantResourceJob()
            {
                Destination = farmlandToPlantWheat,
                ResourceType = resourceCollector.GetResourceTypeToCollect(),
                Unit = resourceCollector
            };
            
            return job;
        }

        private static Job CreatePlantResourceJob(ResourceCollector resourceCollector)
        {
            var locationGameObject = new GameObject("LocationDestination");
            var locationDestination = locationGameObject.AddComponent<LocationDestination>();
            locationDestination.transform.position = GetRandomPointToPlantTree(resourceCollector.GetBuilding() as ResourceBuilding);
            
            Job job = new PlantResourceJob()
            {
                Destination = locationDestination,
                ResourceType = resourceCollector.GetResourceTypeToCollect(),
                Unit = resourceCollector
            };
            
            return job;
        }

        private void AddJobToQueue(Job job)
        {
            switch (job)
            {
                case BuilderJob builderJob:
                    builderJobQueue.AddJob(builderJob);
                    return;
                case VillagerJob villagerJob:
                    villagerJobQueue.AddJob(villagerJob);
                    return;
                case ConsumptionJob consumptionJob:
                    consumptionJobQueue.AddJob(consumptionJob);
                    return;
                case LookingForBuildingJob lookingForBuildingJob:
                    lookingForBuildingJobQueue.AddJob(lookingForBuildingJob);
                    return;
                case CollectResourceJob collectResourceJob:
                    //doesn't need to be added back to anything
                    return;
                default:
                    throw new ArgumentException("JobType not recognized in HandleAddJobToQueue");
            }
        }

        private static bool CheckIfJobNeedsToBeAddedBackToQueue(Unit unit)
        {
            return unit.GetCurrentJob() switch
            {
                null => false,
                ConsumptionJob => true,
                BuilderJob => true,
                LookingForBuildingJob => true,
                VillagerJob => unit is Villager villager && !villager.GetHasResource(),
                CollectResourceJob => unit is ResourceCollector resourceCollector && !resourceCollector.GetHasResource(),
                PlantResourceJob => false,
                _ => throw new InvalidOperationException("Unknown job type")
            };
        }
        
        private void HandleDeleteUnitEvent(IGameEventArgs args)
        {
            if (args is not UnitEventArgs unitEventArgs) return;

            if (CheckIfJobNeedsToBeAddedBackToQueue(unitEventArgs.Unit))
            {
                AddJobToQueue(unitEventArgs.Unit.GetCurrentJob());
            } 
            else
            {
                onDeleteJobEvent.Raise(new JobEventArgs(unitEventArgs.Unit.GetCurrentJob()));
            }
        }

        private static void HandleCompleteJob(IGameEventArgs args)
        {
            if (args is not JobEventArgs jobEventArgs) return;
            jobEventArgs.Job.SetInProgress(false);
        }
        
        private void HandleOnAbortJobEvent(IGameEventArgs args)
        {
            if (args is not JobEventArgs jobEventArgs) return;
            if (CheckIfJobNeedsToBeAddedBackToQueue(jobEventArgs.Job.Unit))
            {
                AddJobToQueue(jobEventArgs.Job);
            } 
        }
    }
}