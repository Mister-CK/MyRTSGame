using Buildings.Model;
using MyRTSGame.Model;
using MyRTSGame.Model.ResourceSystem.Model;
using ResourceSystem.View;
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
        
        public void InjectUnitDependencies(Unit unit)
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
            
            Unit[] allUnits = FindObjectsOfType<Unit>();
            foreach (Unit unit in allUnits)
            {
                unit.unitService = unitService;
            }

        }
        
        private void InjectTerrainService()
        {
            Terrain[] allTerrains = FindObjectsOfType<Terrain>();
            foreach (Terrain terrain in allTerrains)
            {
                terrain.terrainService = terrainService;
            }
        }
        
        private void InjectResourceService()
        {
            NaturalResource[] allResources = FindObjectsOfType<NaturalResource>();
            foreach (NaturalResource resource in allResources)
            {
                resource.resourceService = resourceService;
            }
        }
    }
}
