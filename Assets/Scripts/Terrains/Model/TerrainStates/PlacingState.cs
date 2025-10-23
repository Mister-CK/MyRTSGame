using Enums;
using Interface;
using UnityEngine;

namespace Terrains.Model.TerrainStates
{
    public class PlacingState : ITerrainState
    {
        private readonly TerrainType _terrainType;
        private readonly TerrainManager _terrainManager = TerrainManager.Instance;

        public PlacingState(TerrainType terrainType)
        {
            _terrainType = terrainType;
        }
        
        public void SetObject(Terrain terrain)
        {
            terrain.SetObject(_terrainManager.FoundationObjects[_terrainType]);
            terrain.GetBCollider().size = _terrainManager.FoundationObjects[_terrainType].transform.localScale;
            terrain.GetBCollider().center = _terrainManager.FoundationObjects[_terrainType].transform.localScale / 2;

            var renderer = terrain.GetComponentInChildren<MeshRenderer>();
            terrain.SetMaterial(renderer.material);

            CheckOverlap(terrain);
        }
        
        public static void CheckOverlap(Terrain terrain)
        {
            var transform = terrain.transform;
            var boxSize = terrain.GetBCollider().size - 0.1f * Vector3.one;
            var boxCenter = transform.position + terrain.GetBCollider().center;
            var results = new Collider[16]; // Pre-allocated array assuming we won't ever have more than 16 colliders
            var numColliders = Physics.OverlapBoxNonAlloc(boxCenter, boxSize / 2, results, transform.rotation);

            if (terrain.GetMaterial() != null)
                terrain.GetMaterial().color =
                    numColliders > 2
                        ? Color.red
                        : Color.green; // 2 because the building itself and the ground are also included
            else
                Debug.LogError("Material is null");
        }
    }
}