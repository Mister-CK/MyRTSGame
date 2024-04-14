using System;
using System.Collections.Generic;

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
            building.Inventory = building.InventoryWhenCompleted;
            var inputQuantities = new int[building.InputTypes.Length];
            Array.Fill(inputQuantities, 0);


            var outputWhenCompletedQuantities = building.OutputTypesWhenCompleted != null
                ? new int[building.OutputTypesWhenCompleted.Length]
                : new int[0]; 
            Array.Fill(outputWhenCompletedQuantities, 0);
            
            building.ResourcesInJobForBuilding = Building.InitInventory(building.InputTypes, inputQuantities);
            building.SetOutgoingResources(Building.InitInventory(building.OutputTypesWhenCompleted,
                outputWhenCompletedQuantities));

        }
    }
}