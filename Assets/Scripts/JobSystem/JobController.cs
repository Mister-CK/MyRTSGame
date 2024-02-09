using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace MyRTSGame.Model
{

    public class JobController :  MonoBehaviour
    {
        [SerializeField] private GameEventBuilding onCreateJobsForWarehouse;
        [SerializeField] private GameEventBuilding onNewBuilderJobNeeded;
        
        [SerializeField] private BuilderJobQueue builderJobQueue;
        [SerializeField] private VillagerJobQueue villagerJobQueue;
        
        private static JobController _instance;
        private static BuildingList BuildingList => BuildingList.Instance;
        
        public void Awake() {}
        
        public void CreateJob(VillagerJob villagerJob)
        {
            villagerJob.Destination = FindDestinationForJob(villagerJob);
            villagerJobQueue.AddJob(villagerJob);
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
        
        private void CreateJobsForBuilding(Building building)
        {
            foreach (var resource in building.GetInventory())
            {
                if (resource.Quantity <= 0)
                {
                    continue;
                }
                
                var job = new VillagerJob { Origin = building, ResourceType = resource.ResourceType };
                var destination = FindDestinationForJob(job);
                
                if (destination == null)
                {
                    continue;
                }
                
                job.Destination = destination;
                villagerJobQueue.AddJob(job);
            }
            
        }
        
        private void OnEnable()
        {
            onNewBuilderJobNeeded.RegisterListener(HandleNewJobNeeded);
            onCreateJobsForWarehouse.RegisterListener(CreateJobsForBuilding);
        }

        private void OnDisable()
        {
            onNewBuilderJobNeeded.UnregisterListener(HandleNewJobNeeded);
            onCreateJobsForWarehouse.RegisterListener(CreateJobsForBuilding);

        }

        private void HandleNewJobNeeded(Building building)
        {
            var builderJob = new BuilderJob() {Destination = building};
            builderJobQueue.AddJob(builderJob);
        }
    }
}