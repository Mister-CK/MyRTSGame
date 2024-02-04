using UnityEngine;

namespace MyRTSGame.Model
{
    public class Villager : Unit
    {
        private readonly Resource _resource = new() { ResourceType = ResourceType.Stone, Quantity = 1 };
        private VillagerJob _currentVillagerJob;
        private Building _destination;
        private bool _hasResource;
        private JobQueue _jobQueue;

        protected override void Start()
        {
            base.Start();
            _jobQueue = JobQueue.GetInstance();
        }

        protected override void ExecuteJob()
        {
            if (_hasResource)
                DeliverResource(_destination, _resource.ResourceType);
            else
                TakeResource(_destination, _resource.ResourceType);

            HasDestination = false;
        }

        private void TakeResource(Building building, ResourceType resourceType)
        {
            _hasResource = true;
            building.BuildingController.RemoveResource(resourceType, 1);
        }

        private void DeliverResource(Building building, ResourceType resourceType)
        {
            foreach(var res in building.ResourcesInJobForBuilding)
            {
                if (res.ResourceType == resourceType) res.Quantity--;
            }
            _hasResource = false;
            building.BuildingController.AddResource(resourceType, 1);
        }

        private void PerformNextJob()
        {
            _currentVillagerJob = _jobQueue.GetNextJob();
            if (_currentVillagerJob == null) return;
            _destination = _currentVillagerJob.Origin;
            _resource.ResourceType = _currentVillagerJob.ResourceType;
            Agent.SetDestination(_destination.transform.position);
            HasDestination = true;
        }

        protected override void SetDestination()
        {
            if (!_hasResource)
            {
                PerformNextJob();
                return;
            }
            _destination = _currentVillagerJob.Destination;
            Agent.SetDestination(_destination.transform.position);
            HasDestination = true;
        }

        public VillagerJob GetCurrentJob()
        {
            return _currentVillagerJob;
        }
        
        public bool GetHasDestination()
        {
            return HasDestination;
        }
    }
}