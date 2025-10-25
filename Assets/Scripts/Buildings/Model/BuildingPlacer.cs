using Navigation;
using Unity.AI.Navigation;
using UnityEngine;

namespace Buildings.Model
{
    public class BuildingPlacer : MonoBehaviour
    {
        [SerializeField] private NavMeshSurface navMeshSurface;
        private NavMeshManager _navMeshManager; 
        private Building _building;
        private bool _isPlacing;
        private void Start()
        {
            _navMeshManager = FindObjectOfType<NavMeshManager>();
        }
        private void Update()
        {
            if (!_isPlacing) return;
            
            PlacingState.CheckOverlap(_building);

            // Create a ray from the camera going through the mouse position
            var ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);

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
                    _building.SetState(new FoundationState(_building.GetBuildingType()));
            
                    Bounds boundsToUpdate = GetBuildingBounds(_building);
            
                    _navMeshManager.UpdateNavMesh(boundsToUpdate, navMeshSurface.collectObjects);
                }

            // If the right mouse button is clicked, cancel the placement
            if (Input.GetMouseButtonDown(1))
            {
                _isPlacing = false;
                Destroy(_building.gameObject);
            }
            
        }
        
        private Bounds GetBuildingBounds(Building building)
        {
            Collider buildingCollider = building.GetComponentInChildren<Collider>();
            
            if (buildingCollider != null)
            {
                Bounds bounds = buildingCollider.bounds;
                bounds.Expand(3.0f); // Increased expansion for safety
                return bounds;
            }
            
            return new Bounds(building.transform.position, Vector3.one * 10f);
        }


        public void StartPlacingBuildingFoundation(Building buildingPrefab)
        {
            _isPlacing = true;
            _building = Instantiate(buildingPrefab);
            _building.SetBuildingType(buildingPrefab.GetBuildingType());
            _building.SetState(new PlacingState(_building.GetBuildingType()));
        }
    }
}