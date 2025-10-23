using Buildings.Model;
using Enums;
using Interface;
using MyRTSGame.Model.ResourceSystem.Model.NaturalResources;
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
            
            if (Destination != _building)
            {
                if (CurrentJob is PlantResourceJob) unitService.CreatePlantResourceEvent(CurrentJob);
                if (CurrentJob is CollectResourceJob collectResourceJob)
                {
                    TakeResource(collectResourceJob.Destination, collectResourceJob.ResourceType);
                    if (collectResourceJob.Destination is Wheat wheat) wheat.GetTerrain().SetHasResource(false);
                    if (collectResourceJob.Destination is Grapes grapes) grapes.GetTerrain().SetHasResource(false);
                }
                Destination = _building;
                Agent.SetDestination(Destination.GetPosition());
                HasJobToExecute = true;
                return;
            }

            if (CurrentJob is CollectResourceJob collectResourceJob2)
            {
                DeliverResource(_building, collectResourceJob2.ResourceType);
                unitService.CreateJobNeededEvent(JobType.VillagerJob, null, _building, collectResourceJob2.ResourceType, null);
            }

            unitService.CompleteJob(CurrentJob);
            HasDestination = false;
            CurrentJob = null;
            Destination = null;
        }
        
        private void TakeResource(IDestination destination, ResourceType resourceType)
        {
            _hasResource = true;
            unitService.RemoveResourceFromDestination(destination, resourceType, 1);
        }

        private void DeliverResource(IDestination destination, ResourceType resourceType)
        {
            _hasResource = false;
            unitService.AddResourceToDestination(destination, resourceType, 1);
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