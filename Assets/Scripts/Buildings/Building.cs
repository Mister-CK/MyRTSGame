using System;
using Unity.VisualScripting;
using UnityEngine;

namespace MyRTSGame.Model
{
    public abstract class Building : MonoBehaviour, ISelectable
    {
        [SerializeField] private GameEvent onNewBuilderJobNeeded;
        [SerializeField] private GameEvent onSelectionEvent;

        public bool HasInput;
        private GameObject _buildingObject;
        public int Capacity = 999;
        public int capacityForCompletedBuilding { get; set; }
        public Resource[] Inventory { get; set; }
        public IBuildingState State;
        public Material Material { get; set; }
        public BuildingType BuildingType { get; set; }
        public ResourceType[] InputTypes { get; set; }
        public Resource[] InventoryWhenCompleted { get; set; }
        public ResourceType[] InputTypesWhenCompleted { get; set; }
        public BoxCollider BCollider { get; private set; }
        protected BuildingList BuildingList;
        public Resource[] ResourcesInJobForBuilding { get; set; }
        public int resourceCountNeededForConstruction = 0;
        public BuildingController buildingController;
        
        private void Awake()
        {
            BCollider = this.AddComponent<BoxCollider>();
            BCollider.size = new Vector3(3, 3, 3);
            var resourceTypes = new ResourceType[0];
            var resourceQuantities = new int[0];
            InputTypes = new ResourceType[0];
            Inventory = InitInventory(resourceTypes, resourceQuantities);
            ResourcesInJobForBuilding = InitInventory(resourceTypes, resourceQuantities);
            buildingController = BuildingController.Instance;
        }

        protected virtual void Start()
        {
            
            BuildingList = BuildingList.Instance; 
            State = new PlacingState(BuildingType);
            
            capacityForCompletedBuilding = 5;
            resourceCountNeededForConstruction = 3;
        }

        private void Update()
        {
            if (State is PlacingState placingState) placingState.CheckOverlap(this);
        }

        public void OnMouseDown()
        {
            onSelectionEvent.Raise(new SelectionEventArgs(this));

        }

        public virtual void StartResourceCreationCoroutine()
        {
            // This method can be overridden in derived classes to start the specific coroutine for each building type.
        }

        public void SetState(IBuildingState newState)
        {
            buildingController.SetState(this, newState);
            
            if (newState is ConstructionState)
            {
                onNewBuilderJobNeeded.Raise(new BuildingEventArgs(this));
            }
        }

        public IBuildingState GetState()
        {
            return State;
        }

        public void SetObject(GameObject newObject)
        {
            if (_buildingObject != null) Destroy(_buildingObject);
            _buildingObject = Instantiate(newObject, transform.position + newObject.transform.position,
                Quaternion.identity, transform);
        }

        public static Resource[] InitInventory(ResourceType[] types, int[] quantities)
        {
            if (types.Length != quantities.Length)
                throw new ArgumentException("Types and quantities arrays must have the same length.");

            var resources = new Resource[types.Length];

            for (var i = 0; i < types.Length; i++)
                resources[i] = new Resource
                {
                    ResourceType = types[i],
                    Quantity = quantities[i]
                };

            return resources;
        }

        public Resource[] GetInventory()
        {
            return Inventory;
        }
        
        public Resource[] GetResourcesInJobForBuilding()
        {
            return ResourcesInJobForBuilding;
        }

        public int GetCapacity()
        {
            return Capacity;
        }
    }
}