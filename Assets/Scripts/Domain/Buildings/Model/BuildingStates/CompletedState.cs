using Enums;
using Interface;
using MyRTSGame.Model;
using System.Linq;

namespace Buildings.Model.BuildingStates
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
            building.Capacity = building.CapacityForCompletedBuilding;
            var completedObject = _buildingManager.CompletedObjects[_buildingType];
            building.SetObject(completedObject);
            var localScale = completedObject.transform.localScale;
            building.BCollider.size = localScale;
            building.BCollider.center = localScale / 2;

            building.InputTypes = building.HasInput() ? building.InputTypesWhenCompleted : new ResourceType[0];
            building.SetInventory(InventoryHelper.InitInventory(building.InputTypesWhenCompleted
                .Concat(building.OutputTypesWhenCompleted).ToArray()));
        }
    }
}