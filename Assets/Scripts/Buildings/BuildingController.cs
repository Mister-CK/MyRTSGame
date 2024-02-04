using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace MyRTSGame.Model
{
    public class BuildingController
    {
        private readonly Building _building;
        private readonly JobController _jobController;
        public BuildingController(Building building)
        {
            _jobController = JobController.GetInstance();
            _building = building;
        }

        public void SetState(IBuildingState newState)
        {
            _building.State = newState;

            // if (_building.State is ConstructionState) _building.State = new CompletedState(_building.BuildingType); // skip constructionState
            _building.State.SetObject(_building);

            if (_building.State is CompletedState) _building.StartResourceCreationCoroutine();
        }

        public void AddResource(ResourceType resourceType, int quantity)
        {
            foreach (var resource in _building.Inventory)
            {
                if (resource.ResourceType != resourceType) continue;

                resource.Quantity += quantity;
                if (_building.State is FoundationState foundationState) foundationState.CheckRequiredResources(_building);
                return;
            }

            throw new Exception($"trying to add resource that is not in the inputType ${resourceType}");
        }

        public void RemoveResource(ResourceType resourceType, int quantity)
        {
            foreach (var resource in _building.Inventory)
                if (resource.ResourceType == resourceType)
                {
                    resource.Quantity -= quantity;
                    return;
                }

            throw new Exception("trying to remove resource, but no resource in output has quantity > 0");
        }

        public void TransmuteResource(IEnumerable<Resource> input, IEnumerable<Resource> output)
        {
            foreach (var resource in input) RemoveResource(resource.ResourceType, resource.Quantity);

            foreach (var resource in output) AddResource(resource.ResourceType, resource.Quantity);
        }
        
        public IEnumerator CreateResource(int timeInSeconds, ResourceType resourceType)
        {
            while (true)
            {
                yield return new WaitForSeconds(timeInSeconds);
                var resToCreate = Array.Find(_building.Inventory, resource => resource.ResourceType == resourceType);
                if (resToCreate != null && resToCreate.Quantity < _building.Capacity) AddResource(resourceType, 1);
                _jobController.CreateJob(new VillagerJob { Origin = _building, ResourceType = resourceType });
            }
        }
        
        public IEnumerator CreateOutputFromInput(int intervalInSeconds, Resource[] input, Resource[] output)
        {
            while (true)
            {
                yield return new WaitForSeconds(intervalInSeconds);

                // Check if all required resources are present with quantity > 0
                var hasRequiredResources = input.All(resource => 
                    _building.Inventory.FirstOrDefault(res => res.ResourceType == resource.ResourceType)?.Quantity > 0);

                // Check if all output resources have quantity < capacity
                var isFull = output.All(resource => 
                    _building.Inventory.FirstOrDefault(res => res.ResourceType == resource.ResourceType)?.Quantity >= _building.Capacity);

                if (!hasRequiredResources || isFull) continue;
                
                TransmuteResource(input, output);
                foreach (var resource in output)
                {
                    _jobController.CreateJob(new VillagerJob { Origin = _building, ResourceType = resource.ResourceType });
                }
                
            }
        }
    }
}