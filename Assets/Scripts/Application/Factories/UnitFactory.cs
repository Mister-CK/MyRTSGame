using Buildings.Model;
using Enums;
using System;
using Domain.Units.Component;
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
        private void Awake()
        {

        }
        public void CreateNewUnit(Building trainingBuilding,  UnitType unitType)
        {
            var spawnPosition = trainingBuilding.transform.position + new Vector3(2, 0, -2);
            UnitComponent unit;
            switch (unitType)
            {
                case UnitType.Villager:
                    unit = Instantiate(villagerPrefab, spawnPosition, Quaternion.identity);
                    break;
                case UnitType.Builder:
                    unit = Instantiate(builderPrefab, spawnPosition, Quaternion.identity);
                    break;
                case UnitType.StoneMiner:
                    unit = Instantiate(stoneMinerPrefab, spawnPosition, Quaternion.identity);
                    break;
                case UnitType.LumberJack:
                    unit = Instantiate(lumberJackPrefab, spawnPosition, Quaternion.identity);
                    break;
                case UnitType.Farmer:
                    unit = Instantiate(farmerPrefab, spawnPosition, Quaternion.identity);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(unitType.ToString());
            }
            if (ServiceInjector.Instance != null) ServiceInjector.Instance.InjectUnitDependencies(unit);
        }
    }
}
