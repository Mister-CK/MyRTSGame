using MyRTSGame.Model.Terrains.Model;
using UnityEngine;

namespace MyRTSGame.Model
{
    public class UIBuildTerrainButtonHandler : MonoBehaviour
    {
        [SerializeField] private Terrains.Model.Terrain terrainPrefab;
        [SerializeField] private TerrainPlacer terrainPlacer;

        public void OnButtonClick()
        {
            terrainPlacer.StartPlacingTerrainFoundation(terrainPrefab);
        }
    }
}