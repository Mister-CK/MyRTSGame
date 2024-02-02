using System;

namespace MyRTSGame.Model
{
    public class CompletedState : IBuildingState
    {
        private readonly BuildingList _buildingList = BuildingList.Instance;
        private readonly BuildingManager _buildingManager = BuildingManager.Instance;

        private readonly BuildingType _buildingType;

        public CompletedState(BuildingType buildingType)
        {
            _buildingType = buildingType;
        }

        public void SetObject(Building building)
        {
            var completedObject = _buildingManager.CompletedObjects[_buildingType];
            building.SetObject(completedObject);
            building.BCollider.size = completedObject.transform.localScale;
            building.BCollider.center = completedObject.transform.localScale / 2;

            building.InputTypes = building.HasInput ? building.InputTypesWhenCompleted : new ResourceType[0];
            building.Inventory = (Resource[])building.InventoryWhenCompleted.Clone();
            var inputQuantities = new int[building.InputTypes.Length];
            Array.Fill(inputQuantities, 0);
            building.ResourcesInJobForBuilding = Building.InitInventory(building.InputTypes, inputQuantities);
        }
    }
}