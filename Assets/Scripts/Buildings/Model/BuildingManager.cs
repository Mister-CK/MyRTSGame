using Enums;
using System.Collections.Generic;
using UnityEngine;

namespace  Buildings.Model
{
    public class BuildingManager : MonoBehaviour
    {
        //foundations
        [SerializeField] private GameObject schoolFoundation;
        [SerializeField] private GameObject restaurantFoundation;
        [SerializeField] private GameObject warehouseFoundation;
        [SerializeField] private GameObject stoneQuarryFoundation;
        [SerializeField] private GameObject lumberJackFoundation;
        [SerializeField] private GameObject sawMillFoundation;
        [SerializeField] private GameObject weaponsWorkshopFoundation;
        [SerializeField] private GameObject armorWorkshopFoundation;
        [SerializeField] private GameObject wheatFarmFoundation;
        [SerializeField] private GameObject millFoundation;
        [SerializeField] private GameObject bakeryFoundation;
        [SerializeField] private GameObject pigFarmFoundation;
        [SerializeField] private GameObject butcherFoundation;
        [SerializeField] private GameObject tanneryFoundation;
        [SerializeField] private GameObject goldMineFoundation;
        [SerializeField] private GameObject coalMineFoundation;
        [SerializeField] private GameObject ironMineFoundation;
        [SerializeField] private GameObject goldSmelterFoundation;
        [SerializeField] private GameObject ironSmelterFoundation;
        [SerializeField] private GameObject weaponsSmithFoundation;
        [SerializeField] private GameObject armorSmithFoundation;
        [SerializeField] private GameObject vineyardFoundation;
        [SerializeField] private GameObject fisherMansHutFoundation;
        [SerializeField] private GameObject stableFoundation;
        [SerializeField] private GameObject siegeWorkshopFoundation;
        [SerializeField] private GameObject guardTowerFoundation;
        [SerializeField] private GameObject castleFoundation;

        //completed
        [SerializeField] private GameObject schoolCompleted;
        [SerializeField] private GameObject restaurantCompleted;
        [SerializeField] private GameObject warehouseCompleted;
        [SerializeField] private GameObject stoneQuarryCompleted;
        [SerializeField] private GameObject lumberJackCompleted;
        [SerializeField] private GameObject sawMillCompleted;
        [SerializeField] private GameObject weaponsWorkshopCompleted;
        [SerializeField] private GameObject armorWorkshopCompleted;
        [SerializeField] private GameObject wheatFarmCompleted;
        [SerializeField] private GameObject millCompleted;
        [SerializeField] private GameObject bakeryCompleted;
        [SerializeField] private GameObject pigFarmCompleted;
        [SerializeField] private GameObject butcherCompleted;
        [SerializeField] private GameObject tanneryCompleted;
        [SerializeField] private GameObject goldMineCompleted;
        [SerializeField] private GameObject coalMineCompleted;
        [SerializeField] private GameObject ironMineCompleted;
        [SerializeField] private GameObject goldSmelterCompleted;
        [SerializeField] private GameObject ironSmelterCompleted;
        [SerializeField] private GameObject weaponsSmithCompleted;
        [SerializeField] private GameObject armorSmithCompleted;
        [SerializeField] private GameObject vineyardCompleted;
        [SerializeField] private GameObject fisherMansHutCompleted;
        [SerializeField] private GameObject stableCompleted;
        [SerializeField] private GameObject siegeWorkshopCompleted;
        [SerializeField] private GameObject guardTowerCompleted;
        [SerializeField] private GameObject castleCompleted;

        public Dictionary<BuildingType, GameObject> CompletedObjects;
        public Dictionary<BuildingType, GameObject> FoundationObjects;
        public static BuildingManager Instance { get; private set; }

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
            FoundationObjects = new Dictionary<BuildingType, GameObject>();
            CompletedObjects = new Dictionary<BuildingType, GameObject>();

            // Add the foundation and completed GameObjects for each building type
            FoundationObjects.Add(BuildingType.School, schoolFoundation);
            FoundationObjects.Add(BuildingType.Restaurant, restaurantFoundation);
            FoundationObjects.Add(BuildingType.Warehouse, warehouseFoundation);
            FoundationObjects.Add(BuildingType.StoneQuarry, stoneQuarryFoundation);
            FoundationObjects.Add(BuildingType.LumberJack, lumberJackFoundation);
            FoundationObjects.Add(BuildingType.SawMill, sawMillFoundation);
            FoundationObjects.Add(BuildingType.WeaponsWorkshop, weaponsWorkshopFoundation);
            FoundationObjects.Add(BuildingType.ArmorWorkshop, armorWorkshopFoundation);
            FoundationObjects.Add(BuildingType.WheatFarm, wheatFarmFoundation);
            FoundationObjects.Add(BuildingType.Mill, millFoundation);
            FoundationObjects.Add(BuildingType.Bakery, bakeryFoundation);
            FoundationObjects.Add(BuildingType.PigFarm, pigFarmFoundation);
            FoundationObjects.Add(BuildingType.Butcher, butcherFoundation);
            FoundationObjects.Add(BuildingType.Tannery, tanneryFoundation);
            FoundationObjects.Add(BuildingType.GoldMine, goldMineFoundation);
            FoundationObjects.Add(BuildingType.CoalMine, coalMineFoundation);
            FoundationObjects.Add(BuildingType.IronMine, ironMineFoundation);
            FoundationObjects.Add(BuildingType.GoldSmelter, goldSmelterFoundation);
            FoundationObjects.Add(BuildingType.IronSmelter, ironSmelterFoundation);
            FoundationObjects.Add(BuildingType.WeaponsSmith, weaponsSmithFoundation);
            FoundationObjects.Add(BuildingType.ArmorSmith, armorSmithFoundation);
            FoundationObjects.Add(BuildingType.Vineyard, vineyardFoundation);
            FoundationObjects.Add(BuildingType.FisherMansHut, fisherMansHutFoundation);
            FoundationObjects.Add(BuildingType.Stable, stableFoundation);
            FoundationObjects.Add(BuildingType.SiegeWorkshop, siegeWorkshopFoundation);
            FoundationObjects.Add(BuildingType.GuardTower, guardTowerFoundation);
            FoundationObjects.Add(BuildingType.Castle, castleFoundation);

            CompletedObjects.Add(BuildingType.School, schoolCompleted);
            CompletedObjects.Add(BuildingType.Restaurant, restaurantCompleted);
            CompletedObjects.Add(BuildingType.Warehouse, warehouseCompleted);
            CompletedObjects.Add(BuildingType.StoneQuarry, stoneQuarryCompleted);
            CompletedObjects.Add(BuildingType.LumberJack, lumberJackCompleted);
            CompletedObjects.Add(BuildingType.SawMill, sawMillCompleted);
            CompletedObjects.Add(BuildingType.WeaponsWorkshop, weaponsWorkshopCompleted);
            CompletedObjects.Add(BuildingType.ArmorWorkshop, armorWorkshopCompleted);
            CompletedObjects.Add(BuildingType.WheatFarm, wheatFarmCompleted);
            CompletedObjects.Add(BuildingType.Mill, millCompleted);
            CompletedObjects.Add(BuildingType.Bakery, bakeryCompleted);
            CompletedObjects.Add(BuildingType.PigFarm, pigFarmCompleted);
            CompletedObjects.Add(BuildingType.Butcher, butcherCompleted);
            CompletedObjects.Add(BuildingType.Tannery, tanneryCompleted);
            CompletedObjects.Add(BuildingType.GoldMine, goldMineCompleted);
            CompletedObjects.Add(BuildingType.CoalMine, coalMineCompleted);
            CompletedObjects.Add(BuildingType.IronMine, ironMineCompleted);
            CompletedObjects.Add(BuildingType.GoldSmelter, goldSmelterCompleted);
            CompletedObjects.Add(BuildingType.IronSmelter, ironSmelterCompleted);
            CompletedObjects.Add(BuildingType.WeaponsSmith, weaponsSmithCompleted);
            CompletedObjects.Add(BuildingType.ArmorSmith, armorSmithCompleted);
            CompletedObjects.Add(BuildingType.Vineyard, vineyardCompleted);
            CompletedObjects.Add(BuildingType.FisherMansHut, fisherMansHutCompleted);
            CompletedObjects.Add(BuildingType.Stable, stableCompleted);
            CompletedObjects.Add(BuildingType.SiegeWorkshop, siegeWorkshopCompleted);
            CompletedObjects.Add(BuildingType.GuardTower, guardTowerCompleted);
            CompletedObjects.Add(BuildingType.Castle, castleCompleted);
        }
    }
}