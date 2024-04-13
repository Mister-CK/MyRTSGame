using System;

namespace MyRTSGame.Model
{
    public class CompletedState : IBuildingState
    {
        private readonly BuildingManager _buildingManager = BuildingManager.Instance;

        private readonly BuildingType _buildingType;

        public CompletedState(BuildingType buildingType)
        {
            _buildingType = buildingType;
        }

        public void SetObject(Building building)
        {
            building.Capacity = building.capacityForCompletedBuilding;
            var completedObject = _buildingManager.CompletedObjects[_buildingType];
            building.SetObject(completedObject);
            building.BCollider.size = completedObject.transform.localScale;
            building.BCollider.center = completedObject.transform.localScale / 2;

            building.InputTypes = building.HasInput ? building.InputTypesWhenCompleted : new ResourceType[0];
            building.Inventory = (Resource[])building.InventoryWhenCompleted.Clone();
            var inputQuantities = new int[building.InputTypes.Length];
            Array.Fill(inputQuantities, 0);
            
            var outputWhenCompletedQuantities = new int[building.OutputTypesWhenCompleted.Length];
            Array.Fill(outputWhenCompletedQuantities, 0);

            var inputTypesWhenCompletedQuantities = new int[building.InputTypesWhenCompleted.Length];
            Array.Fill(inputTypesWhenCompletedQuantities, 0);
            
            building.ResourcesInJobForBuilding = Building.InitInventory(building.InputTypes, inputQuantities);
            building.IncomingResources = Building.InitInventory(building.InputTypesWhenCompleted, inputTypesWhenCompletedQuantities);
            building.OutgoingResources = Building.InitInventory(building.OutputTypesWhenCompleted, outputWhenCompletedQuantities);

        }
    }
}