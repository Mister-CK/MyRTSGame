using Interface;
using UnityEngine;

namespace Terrains.Model.TerrainStates
{
    public class PlacingState : ITerrainState
    {
        public static void CheckOverlap(Terrain terrain)
        {
            var transform = terrain.transform;
            var boxSize = terrain.BCollider.size - 0.1f * Vector3.one;
            var boxCenter = transform.position + terrain.BCollider.center;
            var results = new Collider[16]; // Pre-allocated array assuming we won't ever have more than 16 colliders
            var numColliders = Physics.OverlapBoxNonAlloc(boxCenter, boxSize / 2, results, transform.rotation);
            if (terrain.GetMaterial() != null)
                terrain.GetMaterial().color =
                    numColliders > 1
                        ? Color.red
                        : Color.green; 
            else
                Debug.LogError("Material is null");
        }
    }
}