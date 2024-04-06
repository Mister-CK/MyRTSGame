using UnityEngine;

namespace MyRTSGame.Model
{
    public class PlacingState : IBuildingState
    {
        private readonly BuildingManager _buildingManager = BuildingManager.Instance;
        private readonly BuildingType _buildingType;
        private Building _currentBuilding;

        public PlacingState(BuildingType buildingType)
        {
            _buildingType = buildingType;
        }

        public void SetObject(Building building)
        {
            building.SetObject(_buildingManager.FoundationObjects[_buildingType]);
            building.BCollider.size = _buildingManager.FoundationObjects[_buildingType].transform.localScale;
            building.BCollider.center = _buildingManager.FoundationObjects[_buildingType].transform.localScale / 2;

            var renderer = building.GetComponentInChildren<MeshRenderer>();
            building.Material = renderer.material;

            CheckOverlap(building);
            building.InputTypes = new ResourceType[0];
        }

        public void CheckOverlap(Building building)
        {
            var boxSize = building.BCollider.size + new Vector3(2,2,2); // Guarantee a gap of 2 between buildings
            var boxCenter = building.transform.position + building.BCollider.center;
            var colliders = Physics.OverlapBox(boxCenter, boxSize / 2, building.transform.rotation);

            if (building.Material != null)
                building.Material.color =
                    colliders.Length > 2
                        ? Color.red
                        : Color.green; // 2 because the building itself and the ground are also included
            else
                Debug.LogError("Material is null");
        }
    }
}