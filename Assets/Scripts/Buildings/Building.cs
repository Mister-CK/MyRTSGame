using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace MyRTSGame.Model
{
    public abstract class Building : MonoBehaviour, ISelectable
    {
        [SerializeField] private GameEvent onNewBuilderJobNeeded;

        public bool HasInput;
        private GameObject _buildingObject;
        protected JobQueue JobQueue;
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
        protected SelectionManager SelectionManager;
        public Resource[] ResourcesInJobForBuilding { get; set; }
        protected JobController JobController;
        public int resourceCountNeededForConstruction = 0;
        
        public BuildingController BuildingController;
        private void Awake()
        {
            BCollider = this.AddComponent<BoxCollider>();
            BCollider.size = new Vector3(3, 3, 3);
            var resourceTypes = new ResourceType[0];
            var resourceQuantities = new int[0];
            InputTypes = new ResourceType[0];
            Inventory = InitInventory(resourceTypes, resourceQuantities);
            ResourcesInJobForBuilding = InitInventory(resourceTypes, resourceQuantities);
            BuildingController = new BuildingController(this);
        }

        protected virtual void Start()
        {
            JobController = JobController.GetInstance();
            BuildingList = BuildingList.Instance; 
            SelectionManager = SelectionManager.Instance;
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
            OnClick();
        }
        
        private void OnClick()
        {
            if (GetState() is FoundationState foundationState) foundationState.OnClick(this);

            SelectionManager.SelectObject(this);
        }

        public virtual void StartResourceCreationCoroutine()
        {
            // This method can be overridden in derived classes to start the specific coroutine for each building type.
        }

        public void SetState(IBuildingState newState)
        {
            BuildingController.SetState(newState);
            
            if (newState is ConstructionState)
            {
                onNewBuilderJobNeeded.Raise(this);
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