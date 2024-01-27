using System.Collections.Generic;
using MyRTSGame.Model;
using UnityEngine;
namespace MyRTSGame.Model
{
    public class BuildingManager : MonoBehaviour
    {
        public static BuildingManager Instance { get; private set; }


        public Dictionary<BuildingType, GameObject> foundationObjects;
        public Dictionary<BuildingType, GameObject> completedObjects;

        //foundations
        [SerializeField] GameObject warehouseFoundation;
        [SerializeField] GameObject stoneQuarryFoundation;
        [SerializeField] GameObject lumberJackFoundation;
        [SerializeField] GameObject sawMillFoundation;


        //completed
        [SerializeField] GameObject warehouseCompleted;
        [SerializeField] GameObject stoneQuarryCompleted;
        [SerializeField] GameObject lumberJackCompleted;
        [SerializeField] GameObject sawMillCompleted;

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
            foundationObjects = new Dictionary<BuildingType, GameObject>();
            completedObjects = new Dictionary<BuildingType, GameObject>();

            // Add the foundation and completed GameObjects for each building type
            foundationObjects.Add(BuildingType.Warehouse, warehouseFoundation);
            foundationObjects.Add(BuildingType.StoneQuarry, stoneQuarryFoundation);
            foundationObjects.Add(BuildingType.LumberJack, lumberJackFoundation);
            foundationObjects.Add(BuildingType.SawMill, sawMillFoundation);

            completedObjects.Add(BuildingType.Warehouse, warehouseCompleted);
            completedObjects.Add(BuildingType.StoneQuarry, stoneQuarryCompleted);
            completedObjects.Add(BuildingType.LumberJack, lumberJackCompleted);
            completedObjects.Add(BuildingType.SawMill, sawMillCompleted);

        }
    }
}