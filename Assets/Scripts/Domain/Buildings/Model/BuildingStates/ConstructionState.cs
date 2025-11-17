using Buildings.Model.BuildingStates;
using Interface;
using Domain;

namespace Buildings.Model
{
    public class ConstructionState : IBuildingState
    {
        private readonly BuildingManager _buildingManager = BuildingManager.Instance;
        private readonly Building _building;
        private float _percentageCompleted = 0;
        public ConstructionState(Building building)
        {
            _building = building;
        }

        public void SetObject(Building building)
        {
            building.SetObject(_buildingManager.FoundationObjects[_building.GetBuildingType()]);
            building.BCollider.size = _buildingManager.FoundationObjects[_building.GetBuildingType()].transform.localScale;
            building.BCollider.center = _buildingManager.FoundationObjects[_building.GetBuildingType()].transform.localScale / 2;
        }

        public void IncreasePercentageCompleted(float percentage)
        {
            _percentageCompleted += percentage;
            if (_percentageCompleted >= 100)
            {
                _building.SetState(new CompletedState(_building.GetBuildingType()));
            }
        }

        public float GetPercentageCompleted()
        {
            return this._percentageCompleted;
        }
    }
}