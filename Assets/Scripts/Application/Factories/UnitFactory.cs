using Buildings.Model;
using Enums;
using System;
using Units.Model.Component;
using UnityEngine;

namespace Application.Factories
{
    public class UnitFactory: MonoBehaviour
    {
        [SerializeField] private VillagerComponent villagerPrefab;
        [SerializeField] private BuilderComponent builderPrefab;
        [SerializeField] private StoneMinerComponent stoneMinerPrefab;
        [SerializeField] private LumberJackComponent lumberJackPrefab;
        [SerializeField] private FarmerComponent farmerPrefab;
        
        public void CreateNewUnit(Building trainingBuilding,  UnitType unitType)
        {
            var spawnPosition = trainingBuilding.transform.position +
                                new Vector3(2, 0, -2);
            switch (unitType)
            {
                case UnitType.Villager:
                    Instantiate(villagerPrefab, spawnPosition, Quaternion.identity);
                    break;
                case UnitType.Builder:
                    Instantiate(builderPrefab, spawnPosition, Quaternion.identity);
                    break;
                case UnitType.StoneMiner:
                    Instantiate(stoneMinerPrefab, spawnPosition, Quaternion.identity);
                    break;
                case UnitType.LumberJack:
                    Instantiate(lumberJackPrefab, spawnPosition, Quaternion.identity);
                    break;
                case UnitType.Farmer:
                    Instantiate(farmerPrefab, spawnPosition, Quaternion.identity);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(unitType.ToString());
            }
        }
    }
}
