using MyRTSGame.Model;
using MyRTSGame.Interface;

public class PlacingState : IBuildingState
{
    private readonly BuildingManager _buildingManager = BuildingManager.Instance;
    private readonly BuildingType _buildingType;
    private readonly SelectionManager _selectionManager = SelectionManager.Instance;

    public PlacingState(BuildingType buildingType)
    {
        _buildingType = buildingType;
    }

    public void OnClick(Building building)
    {
        building.SetState(new FoundationState(_buildingType));
    }

    public void SetObject(Building building)
    {
        building.SetObject(_buildingManager.FoundationObjects[_buildingType]);
        building.BCollider.size = _buildingManager.FoundationObjects[_buildingType].transform.localScale;
        building.BCollider.center = _buildingManager.FoundationObjects[_buildingType].transform.localScale / 2;
        _selectionManager.SelectObject(building);
    }
}