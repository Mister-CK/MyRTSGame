using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyRTSGame.Model
{
    public class BuildingController : MonoBehaviour
    {

        [SerializeField] private GameEvent onResourceRemovedFromBuilding;
        [SerializeField] private GameEvent onResourceAddedToBuilding;
        [SerializeField] private GameEvent onNewVillagerJobNeeded;
        
        public static BuildingController Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }
        
        private void OnEnable()
        {
            onResourceAddedToBuilding.RegisterListener(OnResourceAdded);
            onResourceRemovedFromBuilding.RegisterListener(OnResourceRemoved);

        }

        private void OnDisable()
        {
            onResourceAddedToBuilding.UnregisterListener(OnResourceAdded);
            onResourceRemovedFromBuilding.UnregisterListener(OnResourceRemoved);
            
        }
        
        private static void OnResourceAdded(IGameEventArgs args)
        {
            if (args is BuildingResourceTypeQuantityEventArgs eventArgs)
            {
                AddResource(eventArgs.Building, eventArgs.ResourceType, eventArgs.Quantity);
            }
        }

        private static void OnResourceRemoved(IGameEventArgs args)
        {
            if (args is BuildingResourceTypeQuantityEventArgs eventArgs)
            {
                RemoveResource(eventArgs.Building, eventArgs.ResourceType, eventArgs.Quantity);
            }
        }

        public void SetState(Building building, IBuildingState newState)
        {
            building.State = newState;

            // if (_building.State is ConstructionState) _building.State = new CompletedState(_building.BuildingType); // skip constructionState
            building.State.SetObject(building);

            if (building.State is CompletedState) building.StartResourceCreationCoroutine();
        }

        private static void AddResource(Building building, ResourceType resourceType, int quantity)
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

        private static void RemoveResource(Building building, ResourceType resourceType, int quantity)
        {
            foreach (var resource in building.Inventory)
                if (resource.ResourceType == resourceType)
                {
                    resource.Quantity -= quantity;
                    return;
                }

            throw new Exception("trying to remove resource, but no resource in output has quantity > 0");
        }

        private static void TransmuteResource(Building building, IEnumerable<Resource> input, IEnumerable<Resource> output)
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
                }
            }
        }
    }
}