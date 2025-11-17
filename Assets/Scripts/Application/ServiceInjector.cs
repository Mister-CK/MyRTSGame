using Application.Services;
using Buildings.Model;
using Domain;
using Domain.Model;
using Domain.Model.ResourceSystem.Model;
using ResourceSystem.View;
using Domain.Units.Component;
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
            unit.OnRequestJob = (u, t) => unitService.CreateUnitJobRequest(u, t);
            unit.OnRequestLookingForBuildingJob = (u) => unitService.CreateNewLookForBuildingJob(u);
            unit.OnJobCompleted = (j) => unitService.CompleteJob(j);
            unit.OnRemoveResourceFromDestination = (u, r, a) => unitService.RemoveResourceFromDestination(u, r, a);
                
            if (unit is ResourceCollectorComponent resourceCollectorComponent)
            {
                resourceCollectorComponent.OnAddResourceToDestination = (u, r, a) => unitService.AddResourceToDestination(u, r, a);
                resourceCollectorComponent.OnCreateJobNeededEvent = (jobtype, destination, origin, resourcetype, unitType) => unitService.CreateJobNeededEvent(jobtype, destination, origin, resourcetype, unitType);
                resourceCollectorComponent.OnCreatePlantResourceEvent = (j) => unitService.CreatePlantResourceEvent(j);
            }
                
            if (unit is VillagerComponent villagerComponent)
            {
                villagerComponent.OnAddResourceToDestination = (u, r, a) => unitService.AddResourceToDestination(u, r, a);
            }
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
            var allBuildings = FindObjectsByType<Building>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        
            foreach (var building in allBuildings)
            {
                building.BuildingService = buildingService;
            }
        }
        
        private void InjectUnitService()
        {
            
            var allUnits = FindObjectsByType<UnitComponent>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (var unit in allUnits) InjectUnitDependencies(unit);
        }
        
        private void InjectTerrainService()
        {
            var allTerrains = FindObjectsByType<Terrain>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (var terrain in allTerrains)
            {
                terrain.terrainService = terrainService;
            }
        }
        
        private void InjectResourceService()
        {
            var allResources = FindObjectsByType<NaturalResource>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (var resource in allResources)
            {
                resource.resourceService = resourceService;
            }
        }
    }
}
