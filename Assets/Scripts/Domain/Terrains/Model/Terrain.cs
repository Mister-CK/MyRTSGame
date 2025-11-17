using Application;
using Application.Services;
using Buildings.Model;
using Enums;
using Interface;
using Unity.VisualScripting;
using UnityEngine;

namespace Terrains.Model
{
    public class Terrain : MonoBehaviour, IDestination, IState<ITerrainState>, IBuildable
    {
        private ITerrainState _state;
        protected TerrainType TerrainType;
        private Material Material { get; set; }
        private BoxCollider BCollider { get; set; }
        private GameObject _terrainObject;
        private readonly float _buildRate = 10f;
        private bool _hasResource;
        protected ResourceType ResourceType;
        public Action<JobType, Terrain, Building, ResourceType?, UnitType?> OnCreateJobNeededEvent { get; set; }
        
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
            _state = new TerrainStates.PlacingState(TerrainType);
        }
        
        public void SetState(ITerrainState terrainState)
        {
            _state = terrainState;
            _state.SetObject(this);
            if (_state is TerrainStates.FoundationState) OnCreateJobNeededEvent?.Invoke(JobType.BuilderJob, this, null, null, null);
        }
        
        public ITerrainState GetState()
        {
            return _state;
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
            return _hasResource;
        }
        
        public void SetHasResource(bool hasResource)
        {
            _hasResource = hasResource;
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