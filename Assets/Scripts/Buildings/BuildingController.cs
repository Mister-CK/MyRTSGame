using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyRTSGame.Model
{
    public class BuildingController : MonoBehaviour
    {

        public static BuildingController Instance { get; private set; }
        [SerializeField] private GameEvent onNewVillagerJobNeeded;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        public void SetState(Building building, IBuildingState newState)
        {
            building.State = newState;

            // if (_building.State is ConstructionState) _building.State = new CompletedState(_building.BuildingType); // skip constructionState
            building.State.SetObject(building);

            if (building.State is CompletedState) building.StartResourceCreationCoroutine();
        }

        public void AddResource(Building building, ResourceType resourceType, int quantity)
        {
            foreach (var resource in building.Inventory)
            {
                if (resource.ResourceType != resourceType) continue;

                resource.Quantity += quantity;
                if (building.State is FoundationState foundationState) foundationState.CheckRequiredResources(building);
                return;
            }

            throw new Exception($"trying to add resource that is not in the inputType ${resourceType}");
        }

        public void RemoveResource(Building building, ResourceType resourceType, int quantity)
        {
            foreach (var resource in building.Inventory)
                if (resource.ResourceType == resourceType)
                {
                    resource.Quantity -= quantity;
                    return;
                }

            throw new Exception("trying to remove resource, but no resource in output has quantity > 0");
        }

        public void TransmuteResource(Building building, IEnumerable<Resource> input, IEnumerable<Resource> output)
        {
            foreach (var resource in input) RemoveResource(building, resource.ResourceType, resource.Quantity);

            foreach (var resource in output) AddResource(building, resource.ResourceType, resource.Quantity);
        }
        
        public IEnumerator CreateResource(Building building, int timeInSeconds, ResourceType resourceType)
        {
            while (true)
            {
                yield return new WaitForSeconds(timeInSeconds);
                var resToCreate = Array.Find(building.Inventory, resource => resource.ResourceType == resourceType);
                if (resToCreate != null && resToCreate.Quantity < building.Capacity) AddResource(building, resourceType, 1);
                onNewVillagerJobNeeded.Raise(new BuildingResourceTypeEventArgs(building, resourceType));
                // _jobController.CreateJob(new VillagerJob { Origin = _building, ResourceType = resourceType });
            }
        }
        
        public IEnumerator CreateOutputFromInput(Building building, int intervalInSeconds, Resource[] input, Resource[] output)
        {
            while (true)
            {
                yield return new WaitForSeconds(intervalInSeconds);

                // Check if all required resources are present with quantity > 0
                var hasRequiredResources = input.All(resource => 
                    building.Inventory.FirstOrDefault(res => res.ResourceType == resource.ResourceType)?.Quantity > 0);

                // Check if all output resources have quantity < capacity
                var isFull = output.All(resource => 
                    building.Inventory.FirstOrDefault(res => res.ResourceType == resource.ResourceType)?.Quantity >= building.Capacity);

                if (!hasRequiredResources || isFull) continue;
                
                TransmuteResource(building, input, output);
                foreach (var resource in output)
                {
                    onNewVillagerJobNeeded.Raise(new BuildingResourceTypeEventArgs(building, resource.ResourceType));
                    // _jobController.CreateJob(new VillagerJob { Origin = _building, ResourceType = resource.ResourceType });
                }
                
            }
        }
    }
}