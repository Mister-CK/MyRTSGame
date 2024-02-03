using Unity.AI.Navigation;
using UnityEngine;

namespace MyRTSGame.Model
{
    public class BuildingPlacer : MonoBehaviour
    {
        [SerializeField] private NavMeshSurface navMeshSurface;
        private Building _building;
        private bool _isPlacing;

        private void Update()
        {
            if (!_isPlacing) return;
            
            var placingState = (PlacingState)_building.GetState();
            placingState.CheckOverlap(_building);

            // Create a ray from the camera going through the mouse position
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Perform the raycast
            if (Physics.Raycast(ray, out var hit))
            {
                // Define the size of the grid
                var gridSize = 1.0f;

                // Round the x and z coordinates to the nearest grid size
                var x = Mathf.Round(hit.point.x / gridSize) * gridSize;
                var z = Mathf.Round(hit.point.z / gridSize) * gridSize;

                // If the ray hits something, move the building to the grid intersection
                _building.transform.position = new Vector3(x, 0, z);
            }

            // If the mouse button is clicked, place the building
            if (Input.GetMouseButtonDown(0))
                if (_building.Material.color == Color.green)
                {
                    _isPlacing = false;
                    _building.SetState(new FoundationState(_building.BuildingType));
                    navMeshSurface.BuildNavMesh();
                }

            // If the right mouse button is clicked, cancel the placement
            if (Input.GetMouseButtonDown(1))
            {
                _isPlacing = false;
                Destroy(_building.gameObject);
            }
            
        }


        public void StartPlacingBuildingFoundation(Building buildingPrefab)
        {
            _isPlacing = true;
            _building = Instantiate(buildingPrefab);
            _building.BuildingType = buildingPrefab.BuildingType;
            _building.SetState(new PlacingState(_building.BuildingType));
        }
    }
}