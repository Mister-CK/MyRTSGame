using MyRTSGame.Model;
using MyRTSGame.Interface;
using Unity.VisualScripting;
using UnityEngine;

public class CompletedState : IBuildingState
{
    private readonly BuildingManager _buildingManager = BuildingManager.Instance;
    private readonly BuildingList _buildingList = BuildingList.Instance;

    private readonly BuildingType _buildingType;

    public CompletedState(BuildingType buildingType)
    {
        _buildingType = buildingType;
    }

    public void OnClick(Building building)
    {
        // Handle click when in CompletedState
    }

    public void SetObject(Building building)
    {
        var completedObject = _buildingManager.CompletedObjects[_buildingType];
        building.SetObject(completedObject);
        building.BCollider.size = completedObject.transform.localScale;
        building.BCollider.center = completedObject.transform.localScale / 2;
        _buildingList.AddBuilding(building);
    }
}