using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MyRTSGame.Model
{
    public abstract class Building : MonoBehaviour, ISelectable
    {
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
        
        private BuildingController _buildingController;
        private void Awake()
        {
            BCollider = this.AddComponent<BoxCollider>();
            BCollider.size = new Vector3(3, 3, 3);
            var resourceTypes = new ResourceType[0];
            var resourceQuantities = new int[0];
            InputTypes = new ResourceType[0];
            Inventory = InitInventory(resourceTypes, resourceQuantities);
            ResourcesInJobForBuilding = InitInventory(resourceTypes, resourceQuantities);
            _buildingController = new BuildingController(this);

        }

        protected virtual void Start()
        {
            State = new PlacingState(BuildingType);
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
            _buildingController.SetState(newState);
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

        public void AddResource(ResourceType resourceType, int quantity)
        {
            _buildingController.AddResource(resourceType, quantity);
        }

        public void RemoveResource(ResourceType resourceType, int quantity)
        {
            _buildingController.RemoveResource(resourceType, quantity);
        }

        public Resource[] GetInventory()
        {
            return Inventory;
        }
        
        public Resource[] GetResourcesInJobForBuilding()
        {
            return ResourcesInJobForBuilding;
        }

        protected void TransmuteResource(Resource[] input, Resource[] output)
        {
            foreach (var resource in input) RemoveResource(resource.ResourceType, resource.Quantity);

            foreach (var resource in output) AddResource(resource.ResourceType, resource.Quantity);
        }

        public int GetCapacity()
        {
            return Capacity;
        }
    }
}