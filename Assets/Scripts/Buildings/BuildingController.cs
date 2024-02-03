using System;

namespace MyRTSGame.Model
{
    public class BuildingController
    {
        private Building _building;

        public BuildingController(Building building)
        {
            _building = building;
        }

        public void SetState(IBuildingState newState)
        {
            _building.State = newState;

            if (_building.State is ConstructionState) _building.State = new CompletedState(_building.BuildingType);
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

        public void TransmuteResource(Resource[] input, Resource[] output)
        {
            foreach (var resource in input) RemoveResource(resource.ResourceType, resource.Quantity);

            foreach (var resource in output) AddResource(resource.ResourceType, resource.Quantity);
        }
    }
}