﻿using Terrains;
using UnityEngine;
using MyTerrain  = Terrains.Model.Terrain;
namespace UI
{
    public class UIBuildTerrainButtonHandler : MonoBehaviour
    {
        [SerializeField] private MyTerrain terrainPrefab;
        [SerializeField] private TerrainPlacer terrainPlacer;

        public void OnButtonClick()
        {
            terrainPlacer.StartPlacingTerrainFoundation(terrainPrefab);
        }
    }
}