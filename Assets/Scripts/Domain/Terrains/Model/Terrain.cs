using Application;
using Application.Services;
using Enums;
using Interface;
using Unity.VisualScripting;
using UnityEngine;

namespace Terrains.Model
{
    public class Terrain : MonoBehaviour, IDestination, IState<ITerrainState>, IBuildable
    {
        protected ITerrainState State;
        protected TerrainType TerrainType;
        protected Material Material { get; set; }
        protected BoxCollider BCollider { get; private set; }
        private GameObject _terrainObject;
        public TerrainService terrainService;
        private readonly float _buildRate = 10f;
        protected bool HasResource;
        protected ResourceType ResourceType;

        public void Awake()
        {
            ServiceInjector.Instance.InjectTerrainDependencies(this);
            BCollider = this.AddComponent<BoxCollider>();
            BCollider.size = new Vector3(1, .1f, 1);
        }
        public Material GetMaterial()
        {
            return Material;
        }
        
        public void SetMaterial(Material material)
        {
            Material = material;
        }
        
        public BoxCollider GetBCollider()
        {
            return BCollider;
        }
        
        public void SetBCollider(BoxCollider bCollider)
        {
            BCollider = bCollider;
        }
        
        public void Start()
        {
            State = new TerrainStates.PlacingState(TerrainType);
        }
        
        public void SetState(ITerrainState terrainState)
        {
            State = terrainState;
            State.SetObject(this);
            if (State is TerrainStates.FoundationState) terrainService.CreateJobNeededEvent(JobType.BuilderJob, this, null, null, null);

            //TerrainController.CreateUpdateViewForBuildingEvent(this);
            //if (State is CompletedState) do something
            //if (State is ConstructionState) TerrainController.CreateJobNeededEvent(JobType.BuilderJob, this, null, null, null);
        }
        
        public ITerrainState GetState()
        {
            return State;
        }
        
        public void SetTerrainType(TerrainType terrainType)
        {
            TerrainType = terrainType;
        }
        
        public TerrainType GetTerrainType()
        {
            return TerrainType;
        }
        
        public void SetObject(GameObject newObject)
        {
            if (_terrainObject != null) Destroy(_terrainObject);
            _terrainObject = Instantiate(newObject, transform.position + newObject.transform.position,
                Quaternion.identity, transform);        
        }
        
        Vector3 IDestination.GetPosition()
        {
            return transform.position;
        }
        
        public float GetBuildRate()
        {
            return _buildRate;
        }

        public bool GetHasResource()
        {
            return HasResource;
        }
        
        public void SetHasResource(bool hasResource)
        {
            HasResource = hasResource;
        }
        
        public ResourceType GetResourceType()
        {
            return ResourceType;
        }
        
        public void SetResourceType(ResourceType resourceType)
        {
            ResourceType = resourceType;
        }
    }
}