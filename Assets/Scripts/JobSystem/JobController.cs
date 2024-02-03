using System;

namespace MyRTSGame.Model
{

    public class JobController
    {
        private JobQueue _jobQueue;
        private BuildingList _buildingList;
        private static JobController _instance;

        private JobController()
        {
            _jobQueue = JobQueue.GetInstance();
            _buildingList = BuildingList.Instance;
        }

        // Public method to get the instance of JobController
        public static JobController GetInstance()
        {
            return _instance ??= new JobController();
        }
        
        
        
        public void CreateJobsForDeliverableResources(Warehouse warehouse)
        {
            foreach (var resource in warehouse.GetInventory())
            {
                if (resource.Quantity > 0)
                {
                    var job = new Job { Origin = warehouse, ResourceType = resource.ResourceType };
                    var destination = FindDestinationForJob(job);
                    if (destination != null)
                    {
                        job.Destination = destination;
                        _jobQueue.AddJob(job);
                    }
                }
            }
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
    }
}