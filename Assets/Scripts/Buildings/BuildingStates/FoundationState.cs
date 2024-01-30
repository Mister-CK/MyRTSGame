using System;
using MyRTSGame.Model;
using MyRTSGame.Interface;
using UnityEngine;

public class FoundationState : IBuildingState
{
    private readonly BuildingList _buildingList = BuildingList.Instance;
    private readonly BuildingManager _buildingManager = BuildingManager.Instance;
    private readonly BuildingType _buildingType;

    public FoundationState(BuildingType buildingType)
    {
        _buildingType = buildingType;
    }

    public void OnClick(Building building)
    {
        building.SetState(new ConstructionState(_buildingType));
    }

    public void CheckRequiredResources(Building building)
    {
        Debug.Log("checking resources");
        Resource[] requiredResources = building.GetRequiredResources();
        Resource[] inventory = building.GetInventory();

        foreach (Resource requiredResource in requiredResources)
        {
            Resource inventoryResource = Array.Find(inventory, resource => resource.ResourceType == requiredResource.ResourceType);
            if (inventoryResource == null || inventoryResource.Quantity < requiredResource.Quantity)
            {
                return;
            }
        }

        // If we reach this point, all required resources are present in the required quantities.
        // We can transition the building to the next state.
        building.SetState(new ConstructionState(_buildingType));
    }
    
    public void SetObject(Building building)
    {
        var foundation = _buildingManager.FoundationObjects[_buildingType];
        building.SetObject(foundation);
        building.BCollider.size = foundation.transform.localScale;
        building.BCollider.center = foundation.transform.localScale / 2;
        _buildingList.AddBuilding(building);

        building.InputTypes = new ResourceType[] { ResourceType.Wood, ResourceType.Stone };
    }
}
