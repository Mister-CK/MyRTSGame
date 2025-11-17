using Enums;
using System.Collections.Generic;
using UnityEngine;

namespace Terrains
{
    public class TerrainManager : MonoBehaviour
    {
        //foundations
        [SerializeField] private GameObject roadFoundation;
        [SerializeField] private GameObject farmlandFoundation;
        [SerializeField] private GameObject vineFoundation;
        
        //completed
        [SerializeField] private GameObject roadCompleted;
        [SerializeField] private GameObject farmlandCompleted;
        [SerializeField] private GameObject vineCompleted;

        public Dictionary<TerrainType, GameObject> CompletedObjects;
        public Dictionary<TerrainType, GameObject> FoundationObjects;
        public static TerrainManager Instance { get; private set; }

        private void Awake()
        {
            // Ensure there is only one instance of BuildingManager
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            FoundationObjects = new Dictionary<TerrainType, GameObject>();
            CompletedObjects = new Dictionary<TerrainType, GameObject>();

            // Add the foundation and completed GameObjects for each building type
            FoundationObjects.Add(TerrainType.Road, roadFoundation);
            FoundationObjects.Add(TerrainType.Farmland, farmlandFoundation);
            FoundationObjects.Add(TerrainType.Vine, vineFoundation);

            CompletedObjects.Add(TerrainType.Road, roadCompleted);
            CompletedObjects.Add(TerrainType.Farmland, farmlandCompleted);
            CompletedObjects.Add(TerrainType.Vine, vineCompleted);

        }
    }
}