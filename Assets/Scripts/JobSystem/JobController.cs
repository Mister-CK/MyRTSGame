using System;
using UnityEngine;

namespace MyRTSGame.Model
{

    public class JobController
    {
        private readonly JobQueue _jobQueue;
        private static JobController _instance;
        private static BuildingList BuildingList => BuildingList.Instance;
        
        private JobController()
        {
            _jobQueue = JobQueue.GetInstance();
        }

        // Public method to get the instance of JobController
        public static JobController GetInstance()
        {
            return _instance ??= new JobController();
        }
        
        public void CreateJob(VillagerJob villagerJob)
        {
            villagerJob.Destination = FindDestinationForJob(villagerJob);
            _jobQueue.AddJob(villagerJob);
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
        
        public void CreateJobsForBuilding(Building building)
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
                _jobQueue.AddJob(job);
            }
            
        }
    }
}