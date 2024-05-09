
using Unity.VisualScripting;
using UnityEngine;

namespace MyRTSGame.Model.Terrains.Model
{
    public class Terrain : MonoBehaviour, IDestination, IState<ITerrainState>
    {
        protected ITerrainState State;
        protected TerrainType TerrainType;
        protected Material Material { get; set; }
        protected BoxCollider BCollider { get; private set; }
        private GameObject _terrainObject;
        protected TerrainController TerrainController; 
        public void Awake()
        {
            BCollider = this.AddComponent<BoxCollider>();
            BCollider.size = new Vector3(1, .1f, 1);
            TerrainController = TerrainController.Instance;
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
            if (State is TerrainStates.FoundationState) TerrainController.CreateJobNeededEvent(JobType.BuilderJob, this, null, null, null);

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
        
        Vector3 MyRTSGame.Model.IDestination.GetPosition()
        {
            return transform.position;
        }
    }
}