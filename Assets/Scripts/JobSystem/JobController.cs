using System;
using UnityEngine;

namespace MyRTSGame.Model
{

    public class JobController
    {
        private JobQueue _jobQueue;
        private static JobController _instance;
        private BuildingList _buildingList => BuildingList.Instance;
        
        private JobController()
        {
            _jobQueue = JobQueue.GetInstance();
        }

        // Public method to get the instance of JobController
        public static JobController GetInstance()
        {
            return _instance ??= new JobController();
        }
        
        public void CreateJob(Job job)
        {
            job.Destination = FindDestinationForJob(job);
            _jobQueue.AddJob(job);
        }
        private Building FindDestinationForJob(Job job)
        {
            var buildings = _buildingList.GetBuildings();
            Building destination = null;
            Building warehouse = null;
            var resourceType = job.ResourceType;

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
                if (resource.Quantity == 0)
                {
                    continue;
                }
                
                var job = new Job { Origin = building, ResourceType = resource.ResourceType };
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