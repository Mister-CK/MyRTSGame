using MyRTSGame.Model;
using MyRTSGame.Interface;

public class CompletedState : IBuildingState
{
    private readonly BuildingManager _buildingManager = BuildingManager.Instance;
    private readonly BuildingList _buildingList = BuildingList.Instance;

    private readonly BuildingType _buildingType;

    public CompletedState(BuildingType buildingType)
    {
        _buildingType = buildingType;
    }

    public void SetObject(Building building)
    {
        var completedObject = _buildingManager.CompletedObjects[_buildingType];
        building.SetObject(completedObject);
        building.BCollider.size = completedObject.transform.localScale;
        building.BCollider.center = completedObject.transform.localScale / 2;

        building.InputTypes = building.HasInput ? building.InputTypesWhenCompleted : new ResourceType[0];
    }
}