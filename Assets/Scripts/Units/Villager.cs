using UnityEngine;

namespace MyRTSGame.Model
{
    public class Villager : Unit
    {
        private readonly Resource _resource = new() { ResourceType = ResourceType.Stone, Quantity = 1 };
        private VillagerJob _currentVillagerJob;
        private bool _hasResource; 
        [SerializeField] private VillagerJobQueue villagerJobQueue;
        protected override void ExecuteJob()
        {
            if (_hasResource)
                DeliverResource(Destination, _resource.ResourceType);
            else
                TakeResource(Destination, _resource.ResourceType);

            HasDestination = false;
        }

        private void TakeResource(Building building, ResourceType resourceType)
        {
            _hasResource = true;
            BuildingController.RemoveResource(building, resourceType, 1);
        }

        private void DeliverResource(Building building, ResourceType resourceType)
        {
            foreach(var res in building.ResourcesInJobForBuilding)
            {
                if (res.ResourceType == resourceType) res.Quantity--;
            }
            _hasResource = false;
            BuildingController.AddResource(building, resourceType, 1);
        }

        private void PerformNextJob()
        {
            _currentVillagerJob = villagerJobQueue.GetNextJob();
            if (_currentVillagerJob == null) return;
            Destination = _currentVillagerJob.Origin;
            _resource.ResourceType = _currentVillagerJob.ResourceType;
            Agent.SetDestination(Destination.transform.position);
            HasDestination = true;
        }

        protected override void SetDestination()
        {
            if (!_hasResource)
            {
                PerformNextJob();
                return;
            }
            Destination = _currentVillagerJob.Destination;
            Agent.SetDestination(Destination.transform.position);
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