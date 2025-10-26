using Application.Services;
using Buildings.Model;
using MyRTSGame.Model;
using MyRTSGame.Model.ResourceSystem.Model;
using ResourceSystem.View;
using Units.Model.Component;
using UnityEngine;
using Terrain = Terrains.Model.Terrain;

namespace Application
{
    public class ServiceInjector : MonoBehaviour
    {
        [SerializeField] private BuildingService buildingService;
        [SerializeField] private JobService jobService;
        [SerializeField] private UnitService unitService;
        [SerializeField] private TerrainService terrainService;
        [SerializeField] private ResourceService resourceService;
        
        public static ServiceInjector Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        [ContextMenu("Inject All Dependencies")]
        private void Start()
        {
            InjectBuildingService();
            InjectUnitService();
            InjectTerrainService();
            InjectResourceService();
        }

        public void InjectBuildingDependenies(Building building)
        {
            building.BuildingService = buildingService;
        }
        
        public void InjectUnitDependencies(UnitComponent unit)
        {
            unit.unitService = unitService;
        }
        
        public void InjectUnitDependencies(UnitView unitView)
        {
            unitView.unitService = unitService;
        }

        public void InjectResourceDependencies(NaturalResource naturalResource)
        {
            naturalResource.resourceService = resourceService;
        }
        
        public void InjectTerrainDependencies(Terrain terrain)
        {
            terrain.terrainService = terrainService;
        }
        
        public void InjectNaturalResourceViewDependencies(NaturalResourceView resourceView)
        {
            resourceView.resourceService = resourceService;
        }
        
        private void InjectBuildingService()
        {
            Building[] allBuildings = FindObjectsOfType<Building>();
        
            foreach (Building building in allBuildings)
            {
                building.BuildingService = buildingService;
            }
        }
        
        private void InjectUnitService()
        {
            
            UnitComponent[] allUnits = FindObjectsOfType<UnitComponent>();
            foreach (UnitComponent unit in allUnits)
            {
                unit.unitService = unitService;
            }

        }
        
        private void InjectTerrainService()
        {
            var allTerrains = FindObjectsOfType<Terrain>();
            foreach (var terrain in allTerrains)
            {
                terrain.terrainService = terrainService;
            }
        }
        
        private void InjectResourceService()
        {
            var allResources = FindObjectsOfType<NaturalResource>();
            foreach (var resource in allResources)
            {
                resource.resourceService = resourceService;
            }
        }
    }
}
