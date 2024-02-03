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
        protected int Capacity = 999;
        public Resource[] Inventory { get; set; }
        protected IBuildingState State;
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
        private void Awake()
        {
            BCollider = this.AddComponent<BoxCollider>();
            BCollider.size = new Vector3(3, 3, 3);
            var resourceTypes = new ResourceType[0];
            var resourceQuantities = new int[0];
            InputTypes = new ResourceType[0];
            Inventory = InitInventory(resourceTypes, resourceQuantities);
            ResourcesInJobForBuilding = InitInventory(resourceTypes, resourceQuantities);
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

        public abstract IEnumerable<Resource> GetRequiredResources();

        private void OnClick()
        {
            if (GetState() is FoundationState foundationState) foundationState.OnClick(this);

            SelectionManager.SelectObject(this);
        }

        protected virtual void StartResourceCreationCoroutine()
        {
            // This method can be overridden in derived classes to start the specific coroutine for each building type.
        }

        public void SetState(IBuildingState newState)
        {
            State = newState;

            // immediately transition to completed state
            if (State is ConstructionState) State = new CompletedState(BuildingType);
            State.SetObject(this);


            if (State is CompletedState) StartResourceCreationCoroutine();
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
            foreach (var resource in Inventory)
            {
                if (resource.ResourceType != resourceType) continue;

                resource.Quantity += quantity;
                if (State is FoundationState foundationState) foundationState.CheckRequiredResources(this);
                return;
            }

            throw new Exception($"trying to add resource that is not in the inputType ${resourceType}");
        }

        public void RemoveResource(ResourceType resourceType, int quantity)
        {
            foreach (var resource in Inventory)
                if (resource.ResourceType == resourceType)
                {
                    resource.Quantity -= quantity;
                    return;
                }

            throw new Exception("trying to remove resource, but no resource in output has quantity > 0");
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