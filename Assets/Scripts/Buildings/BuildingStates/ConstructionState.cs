using MyRTSGame.Model;
using MyRTSGame.Interface;
using Unity.VisualScripting;
using UnityEngine;


public class ConstructionState : IBuildingState
{
    private readonly BuildingManager _buildingManager = BuildingManager.Instance;
    private readonly BuildingType _buildingType;
    
    public ConstructionState(BuildingType buildingType)
    {
        _buildingType = buildingType;
    }
    public void OnClick(Building building)
    {
        building.SetState(new CompletedState(_buildingType));
    }

    public void SetObject(Building building)
    {
        building.SetObject(_buildingManager.FoundationObjects[_buildingType]);
        building.BCollider.size = _buildingManager.FoundationObjects[_buildingType].transform.localScale;
        building.BCollider.center = _buildingManager.FoundationObjects[_buildingType].transform.localScale / 2;
    }
}