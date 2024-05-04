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
        public bool GetHasResource()
        {
            return _hasResource;
        }

        public void SetHasResource(bool hasResource)
        {
            _hasResource = hasResource;
        }
        
        
        public ResourceType GetResourceTypeToCollect()
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
            unitController.CreateJobNeededEvent(JobType.VillagerJob, null, _building, collectResourceJob.ResourceType, null);
            unitController.CompleteJob(CurrentJob);
            HasDestination = false;
            CurrentJob = null;
            Destination = null;
        }
        
        private void TakeResource(IDestination destination, ResourceType resourceType)
        {
            _hasResource = true;
            unitController.RemoveResourceFromDestination(destination, resourceType, 1);
        }

        private void DeliverResource(IDestination destination, ResourceType resourceType)
        {
            _hasResource = false;
            unitController.AddResourceToDestination(destination, resourceType, 1);
        }
        
        public void BuildingDeleted()
        {
            HasDestination = false;
            SetHasResource(false);
            SetBuilding(null);
            SetIsLookingForBuilding(true);
            SetCurrentJob(null);
            Agent.SetDestination(Agent.transform.position);
        }
    }
}