using System;
using MyRTSGame.Model;
using MyRTSGame.Interface;
using Unity.VisualScripting;
using UnityEngine;

public class FoundationState : IBuildingState
{
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

    public void SetObject(Building building)
    {
        var foundation = _buildingManager.FoundationObjects[_buildingType];
        building.SetObject(foundation);
        building.BCollider.size = foundation.transform.localScale;
        building.BCollider.center = foundation.transform.localScale / 2;

    }
}
