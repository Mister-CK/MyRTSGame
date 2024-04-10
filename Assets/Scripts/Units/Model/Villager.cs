using UnityEngine;

namespace MyRTSGame.Model
{
    public class Villager : Unit
    {
        private Resource _resource = new() { ResourceType = ResourceType.Stone, Quantity = 1 };
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
            unitController.RemoveResourceFromBuilding(building, resourceType, 1);
        }

        private void DeliverResource(Building building, ResourceType resourceType)
        {
            foreach(var res in building.ResourcesInJobForBuilding)
            {
                if (res.ResourceType == resourceType) res.Quantity--;
            }
            _hasResource = false;
            unitController.AddResourceToBuilding(building, resourceType, 1);
        }

        private void PerformNextJob()
        {
            _currentVillagerJob = villagerJobQueue.GetNextJob();
            if (_currentVillagerJob == null) return;
            _currentVillagerJob.SetInProgress(true);
            Destination = _currentVillagerJob.Origin;
            _resource.ResourceType = _currentVillagerJob.ResourceType;
            Agent.SetDestination(Destination.transform.position);
            HasDestination = true;
        }

        private void RequestNewJob()
        {
            unitController.CreateVillagerJobRequest(this);
        }
        
        public void AcceptNewVillagerJob(VillagerJob villagerJob)
        {
            _currentVillagerJob = villagerJob;
            Destination = _currentVillagerJob.Origin;
            _resource.ResourceType = _currentVillagerJob.ResourceType;
            Agent.SetDestination(Destination.transform.position);
            HasDestination = true;
        }

        public void UnAssignVillagerJob(DestinationType destinationType)
        {
            
            if (destinationType == DestinationType.Origin && _hasResource)
            {
                return;
            }

            HasDestination = false;
            _currentVillagerJob = null;
            _hasResource = false;
            Destination = null;

            SetDestination();
        }
        
        protected override void SetDestination()
        {
            if (!_hasResource)
            {
                RequestNewJob();
                return;
            }
            Destination = _currentVillagerJob.Destination;
            Agent.SetDestination(Destination.transform.position);
            HasDestination = true;
        }
    }
}