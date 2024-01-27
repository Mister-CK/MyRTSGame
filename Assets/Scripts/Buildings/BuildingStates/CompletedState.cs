using MyRTSGame.Model;
using MyRTSGame.Interface;

public class CompletedState : IBuildingState
{
    readonly BuildingManager _buildingManager = BuildingManager.Instance;
    readonly BuildingList _buildingList = BuildingList.Instance;

    public BuildingType buildingType;

    public CompletedState(BuildingType buildingType)
    {
        this.buildingType = buildingType;
    }

    public void OnClick(Building building)
    {
        // Handle click when in CompletedState
    }

    public void SetObject(Building building)
    {
        building.SetObject(_buildingManager.completedObjects[buildingType]);
        _buildingList.AddBuilding(building);
    }
}