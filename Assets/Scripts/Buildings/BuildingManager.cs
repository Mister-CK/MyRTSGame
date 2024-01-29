using System.Collections.Generic;
using MyRTSGame.Model;
using UnityEngine;
namespace MyRTSGame.Model
{
    public class BuildingManager : MonoBehaviour
    {
        public static BuildingManager Instance { get; private set; }


        public Dictionary<BuildingType, GameObject> FoundationObjects;
        public Dictionary<BuildingType, GameObject> CompletedObjects;

        //foundations
        [SerializeField] private GameObject warehouseFoundation;
        [SerializeField] private GameObject stoneQuarryFoundation;
        [SerializeField] private GameObject lumberJackFoundation;
        [SerializeField] private GameObject sawMillFoundation;


        //completed
        [SerializeField] private GameObject warehouseCompleted;
        [SerializeField] private GameObject stoneQuarryCompleted;
        [SerializeField] private GameObject lumberJackCompleted;
        [SerializeField] private GameObject sawMillCompleted;

        private void Awake()
        {
            // Ensure there is only one instance of BuildingManager
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            FoundationObjects = new Dictionary<BuildingType, GameObject>();
            CompletedObjects = new Dictionary<BuildingType, GameObject>();

            // Add the foundation and completed GameObjects for each building type
            FoundationObjects.Add(BuildingType.Warehouse, warehouseFoundation);
            FoundationObjects.Add(BuildingType.StoneQuarry, stoneQuarryFoundation);
            FoundationObjects.Add(BuildingType.LumberJack, lumberJackFoundation);
            FoundationObjects.Add(BuildingType.SawMill, sawMillFoundation);

            CompletedObjects.Add(BuildingType.Warehouse, warehouseCompleted);
            CompletedObjects.Add(BuildingType.StoneQuarry, stoneQuarryCompleted);
            CompletedObjects.Add(BuildingType.LumberJack, lumberJackCompleted);
            CompletedObjects.Add(BuildingType.SawMill, sawMillCompleted);
        }
    }
}