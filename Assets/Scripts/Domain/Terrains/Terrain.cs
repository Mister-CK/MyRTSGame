using Buildings.Model;
using Enums;
using Interface;
using Terrains.Model.TerrainStates;
using Unity.VisualScripting;
using UnityEngine;

namespace Terrains.Model
{
    public class Terrain : MonoBehaviour, IDestination, IState<ITerrainState>, IBuildable
    {
        private ITerrainState _state;
        protected TerrainType TerrainType;
        private Material Material { get; set; }
        public BoxCollider BCollider { get; set; }
        public GameObject FoundationObject { get; set; }
        public GameObject CompletedObject { get; set; }
        public Color FoundationColor { get; set; }
        private readonly float _buildRate = 10f;
        private bool _hasResource;
        protected ResourceType ResourceType;
        public Action<JobType, Terrain, Building, ResourceType?, UnitType?> OnCreateJobNeededEvent { get; set; }

        public void SetState(ITerrainState terrainState)
        {
            _state = terrainState;
            if (_state is TerrainStates.FoundationState)
            {
                OnCreateJobNeededEvent?.Invoke(JobType.BuilderJob, this, null, null, null);
                Material.color = FoundationColor;
            }
            if (_state is CompletedState) UpdateView();
        }
        
        private void UpdateView()
        {
            if (_state is CompletedState)
            {
                FoundationObject.SetActive(false);
                CompletedObject.SetActive(true);
            }   
            else
            {
                FoundationObject.SetActive(true);
                CompletedObject.SetActive(false);
            }   
        }
        
        public Material GetMaterial() => Material;
        public void SetMaterial(Material material) => Material = material;
        
        public ITerrainState GetState() => _state;
        public TerrainType GetTerrainType() =>TerrainType;
        Vector3 IDestination.GetPosition() => transform.position;
        public float GetBuildRate() =>  _buildRate;
        public bool GetHasResource() => _hasResource; 
        
        public void SetHasResource(bool hasResource) => _hasResource = hasResource;
        public ResourceType GetResourceType() => ResourceType;
    }
}