using UnityEngine;

namespace MyRTSGame.Model
{
    public class ResourceCollector: Unit
    {
        protected ResourceType ResourceTypeToCollect;
        private Building _building;
        private bool _hasResource;
        public Building GetBuilding()
        {
            return _building;
        }
        
        public void SetBuilding(Building building)
        {
            _building = building;
        }
        
        public ResourceType GetResourceToCollect()
        {
            return ResourceTypeToCollect;
        }

        public void SetResourceTypeToCollect(ResourceType resourceType)
        {
            ResourceTypeToCollect = resourceType;
        }
        
        protected override void ExecuteJob()
        {
            base.ExecuteJob();
            if (CurrentJob is not CollectResourceJob collectResourceJob) return;
            
            if (!_hasResource)
            {
                TakeResource(collectResourceJob.Destination, collectResourceJob.ResourceType);
                Destination = _building;
                Agent.SetDestination(Destination.GetPosition());
                return;
            }
            DeliverResource(_building, collectResourceJob.ResourceType);
            HasDestination = false;
        }
        
        private void TakeResource(IDestination destination, ResourceType resourceType)
        {
            _hasResource = true;
            unitController.RemoveResourceFromBuilding(destination, resourceType, 1);
        }

        private void DeliverResource(IDestination destination, ResourceType resourceType)
        {
            _hasResource = false;
            Debug.Log("ADd Resource to building" + destination.GetBuildingType()  + " " + resourceType);
            unitController.AddResourceToBuilding(destination, resourceType, 1);
        }
    }
}