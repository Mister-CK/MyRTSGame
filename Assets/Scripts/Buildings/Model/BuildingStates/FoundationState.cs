using System;
using UnityEngine;

namespace MyRTSGame.Model
{
    public class FoundationState : IBuildingState
    {
        private readonly BuildingList _buildingList = BuildingList.Instance;
        private readonly BuildingManager _buildingManager = BuildingManager.Instance;
        private readonly BuildingType _buildingType;

        public FoundationState(BuildingType buildingType)
        {
            _buildingType = buildingType;
        }

        public void SetObject(Building building)
        {
            building.Capacity = building.resourceCountNeededForConstruction;
            var foundation = _buildingManager.FoundationObjects[_buildingType];
            building.SetObject(foundation);
            building.BCollider.size = foundation.transform.localScale;
            building.BCollider.center = foundation.transform.localScale / 2;
            building.InputTypes = new[] { ResourceType.Wood, ResourceType.Stone };
            building.Inventory = Building.InitInventory(building.InputTypes);

            _buildingList.AddBuilding(building);
        }

        public void CheckRequiredResources(Building building)
        {
            var requiredResources = new[] { ResourceType.Wood, ResourceType.Stone };
            var inventory = building.GetInventory();
            
            foreach (var requiredResource in requiredResources)
            {
                var inventoryResource = inventory[requiredResource].Current;
                if (inventoryResource < building.resourceCountNeededForConstruction) return;
            }

            // If we reach this point, all required resources are present in the required quantities.
            // We can transition the building to the next state.
            Debug.Log("Set buildingState to ConstructionState for building: " + building.BuildingType);
            building.SetState(new ConstructionState(_buildingType));
        }
    }
}