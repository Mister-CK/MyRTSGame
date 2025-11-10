using Terrains.Model.TerrainStates;
using UnityEngine;
using MyTerrain = Terrains.Model.Terrain;
namespace Terrains
{
    public class TerrainPlacer : MonoBehaviour
    {
        private MyTerrain _terrain;
        private MyTerrain _terrainPrefab;
        private bool _isPlacing;
        private void Update()
        {
            if (!_isPlacing) return;
            
            PlacingState.CheckOverlap(_terrain);

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
                _terrain.transform.position = new Vector3(x, 0, z);
            }

            // If the mouse button is clicked, place the building
            if (Input.GetMouseButtonDown(0))
                if (_terrain.GetMaterial().color == Color.green)
                {
                    _isPlacing = false;
                    _terrain.SetState(new FoundationState(_terrain.GetTerrainType()));
                    StartPlacingTerrainFoundation(_terrainPrefab);
                    PlacingState.CheckOverlap(_terrain);
                }

            // If the right mouse button is clicked, cancel the placement
            if (Input.GetMouseButtonDown(1))
            {
                _isPlacing = false;
                Destroy(_terrain.gameObject);
            }
            
        }


        public void StartPlacingTerrainFoundation(MyTerrain terrainPrefab)
        {
            _isPlacing = true;
            _terrainPrefab = terrainPrefab;
            _terrain = Instantiate(terrainPrefab);
            _terrain.SetTerrainType(terrainPrefab.GetTerrainType());
            _terrain.SetState(new PlacingState(_terrain.GetTerrainType()));
        }
    }
}