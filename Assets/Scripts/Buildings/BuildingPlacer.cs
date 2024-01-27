using UnityEngine;
using MyRTSGame.Model;

public class BuildingPlacer : MonoBehaviour
{
    private bool _isPlacing = false;
    private Building _building;


    public void StartPlacingBuildingFoundation(Building buildingPrefab)
    {
        _isPlacing = true;
        _building = Instantiate(buildingPrefab);
        _building.SetState(new FoundationState(_building.GetBuildingType()));
    }

    void Update()
    {
        if (_isPlacing)
        {
            // Create a ray from the camera going through the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Perform the raycast
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Define the size of the grid
                float gridSize = 1.0f;

                // Round the x and z coordinates to the nearest grid size
                float x = Mathf.Round(hit.point.x / gridSize) * gridSize;
                float z = Mathf.Round(hit.point.z / gridSize) * gridSize;

                // If the ray hits something, move the building to the grid intersection
                _building.transform.position = new Vector3(x, 0, z);
            }

            // If the mouse button is clicked, place the building
            if (Input.GetMouseButtonDown(0))
            {
                _isPlacing = false;
                _building.SetState(new CompletedState(_building.GetBuildingType()));
            }
            
            // If the right mouse button is clicked, cancel the placement
            if (Input.GetMouseButtonDown(1))
            {
                _isPlacing = false;
                Destroy(_building.gameObject);
            }
        }
    }
}