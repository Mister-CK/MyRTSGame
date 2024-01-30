using MyRTSGame.Model;
using MyRTSGame.Interface;
using UnityEngine;

public class PlacingState : IBuildingState
{
    private readonly BuildingManager _buildingManager = BuildingManager.Instance;
    private readonly BuildingType _buildingType;
    private readonly SelectionManager _selectionManager = SelectionManager.Instance;
    private Building _currentBuilding;

    public PlacingState(BuildingType buildingType)
    {
        _buildingType = buildingType;
    }

    public void OnClick(Building building)
    {
        if (building.Material.color == Color.green)
        {
            building.SetState(new FoundationState(_buildingType));
        }    
    }

    public void SetObject(Building building)
    {
        building.SetObject(_buildingManager.FoundationObjects[_buildingType]);
        building.BCollider.size = _buildingManager.FoundationObjects[_buildingType].transform.localScale;
        building.BCollider.center = _buildingManager.FoundationObjects[_buildingType].transform.localScale / 2;

        MeshRenderer renderer = building.GetComponentInChildren<MeshRenderer>();
        building.Material = renderer.material;

        CheckOverlap(building);
        _selectionManager.SelectObject(building);
    }
    
    public void CheckOverlap(Building building)
    {
        var boxSize = building.BCollider.size;
        var boxCenter = building.transform.position + building.BCollider.center;

        var colliders = Physics.OverlapBox(boxCenter, boxSize / 2, building.transform.rotation);
        foreach (var collider in colliders)
        {
            Debug.Log(collider);
            Debug.Log(collider.name);

        }
        if (building.Material != null)
        {
            building.Material.color = colliders.Length > 2 ? Color.red : Color.green; // 2 because the building itself and the ground are also included
        }
        else
        {
            Debug.LogError("Material is null");
        }

    }
}