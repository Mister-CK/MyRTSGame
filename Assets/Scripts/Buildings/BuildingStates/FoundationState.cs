using System;
using MyRTSGame.Model;
using MyRTSGame.Interface;
using UnityEngine;

public class FoundationState : IBuildingState
{
    readonly BuildingManager _buildingManager = BuildingManager.Instance;


    public BuildingType buildingType;

    public FoundationState(BuildingType buildingType)
    {
        this.buildingType = buildingType;
    }

    public void OnClick(Building building)
    {
        // Transition from FoundationState to ConstructionState
        building.SetState(new ConstructionState());
    }

    public void SetObject(Building building)
    {
        // Set the GameObject to a foundation
        building.SetObject(_buildingManager.foundationObjects[buildingType]);
    }
}
