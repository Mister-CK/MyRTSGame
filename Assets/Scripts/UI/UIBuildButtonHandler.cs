using Buildings.Model;
using UnityEngine;

namespace UI
{
    public class UIBuildButtonHandler : MonoBehaviour
    {
        [SerializeField] private Building buildingPrefab;
        [SerializeField] private BuildingPlacer buildingPlacer;

        public void OnButtonClick()
        {
            buildingPlacer.StartPlacingBuildingFoundation(buildingPrefab);
        }
    }
}