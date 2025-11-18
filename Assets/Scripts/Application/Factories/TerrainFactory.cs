using Data;
using Unity.VisualScripting;
using UnityEngine;
using Terrain = Terrains.Model.Terrain;

namespace Application.Factories
{
    public class TerrainFactory: MonoBehaviour
    {
        public Terrain CreateNewTerrain(Terrain prefab)
        {
            var terrain = Instantiate(prefab);

            terrain.BCollider = terrain.AddComponent<BoxCollider>();
            terrain.BCollider.size = TerrainManager.Instance.FoundationObjects[terrain.GetTerrainType()].transform.localScale;
            terrain.BCollider.center = TerrainManager.Instance.FoundationObjects[terrain.GetTerrainType()].transform.localScale / 2;
            
            terrain.FoundationObject = Instantiate(TerrainManager.Instance.FoundationObjects[terrain.GetTerrainType()], terrain.transform);
            terrain.FoundationObject.SetActive(true);
            terrain.CompletedObject = Instantiate(TerrainManager.Instance.CompletedObjects[terrain.GetTerrainType()], terrain.transform);
            terrain.CompletedObject.SetActive(false);
            
            var mashRenderer = terrain.FoundationObject.GetComponentInChildren<MeshRenderer>();
            terrain.SetMaterial(mashRenderer.material);
            terrain.FoundationColor = new Color(mashRenderer.sharedMaterial.color.r, mashRenderer.sharedMaterial.color.g, mashRenderer.sharedMaterial.color.b);

            if (ServiceInjector.Instance != null) ServiceInjector.Instance.InjectTerrainDependencies(terrain);

            
            return terrain;
        }
        public GameObject CreateCompletedTerrain(Terrain terrain)
        {
            var completedObject = TerrainManager.Instance.CompletedObjects[terrain.GetTerrainType()];

            return Instantiate(completedObject, transform.position + completedObject.transform.position,
                Quaternion.identity, transform);   
        }
        
  
    }
}
