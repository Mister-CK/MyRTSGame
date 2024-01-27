using UnityEngine;
namespace MyRTSGame.Model
{
    public class UIButtonHandler : MonoBehaviour
    {
        [SerializeField] private Building buildingPrefab;
        [SerializeField] private BuildingPlacer buildingPlacer;

        public void OnButtonClick()
        {
            buildingPlacer.StartPlacingBuildingFoundation(buildingPrefab);
        }
    }
}