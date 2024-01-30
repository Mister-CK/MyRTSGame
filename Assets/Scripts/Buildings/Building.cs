using System;
using UnityEngine;
using MyRTSGame.Interface;
using Unity.VisualScripting;

namespace MyRTSGame.Model
{

    public abstract class Building : MonoBehaviour, ISelectable
    {
        public bool HasInput = false;
        private SelectionManager _selectionManager;
        public abstract Resource[] GetRequiredResources();
        protected IBuildingState State;
        public BuildingType BuildingType { get; protected set; }
        public ResourceType[] InputTypes { get; set; }
        public ResourceType[] InputTypesWhenCompleted { get; set; }
        protected Resource[] Inventory;
        protected int Capacity = 999;
        public BoxCollider BCollider { get; private set; }
        private GameObject _buildingObject;
        private JobQueue _jobQueue;
        
        void Awake()
        {
            BCollider = this.AddComponent<BoxCollider>();
            BCollider.size = new Vector3(3, 3, 3);
            _jobQueue = JobQueue.GetInstance();
            ResourceType[] resourceTypes = new ResourceType[0];
            int[] resourceQuantities = new int[0];
            InputTypes = new ResourceType[0];
            Inventory = InitInventory(resourceTypes, resourceQuantities);
            _selectionManager = SelectionManager.Instance;
        }
        
        protected virtual void Start()
        {
            State = new PlacingState(BuildingType);
        }
        public void OnMouseDown()
        {
            OnClick();
        }
        private void OnClick()
        {
            State.OnClick(this);
            _selectionManager.SelectObject(this);
        }

        protected virtual void StartResourceCreationCoroutine()
        {
            // This method can be overridden in derived classes to start the specific coroutine for each building type.
        }
        
        public void SetState(IBuildingState newState)
        {
            State = newState;
            State.SetObject(this);
            if (State is CompletedState)
            {
                StartResourceCreationCoroutine();
            }
        }
        
        public IBuildingState GetState()
        {
            return State;
        }

        public void SetObject(GameObject newObject)
        {
            if (_buildingObject != null)
            {
                Destroy(_buildingObject);
            }
            _buildingObject = Instantiate(newObject, transform.position + newObject.transform.position, Quaternion.identity, transform);
        }

        protected static Resource[] InitInventory(ResourceType[] types, int[] quantities)
        {
            if (types.Length != quantities.Length)
            {
                throw new ArgumentException("Types and quantities arrays must have the same length.");
            }

            Resource[] resources = new Resource[types.Length];

            for (int i = 0; i < types.Length; i++)
            {
                resources[i] = new Resource()
                {
                    ResourceType = types[i],
                    Quantity = quantities[i]
                };
            }

            return resources;
        }

        public void AddResource(ResourceType resourceType, int quantity)
        {
            Debug.Log("AddResource");
            foreach(var resource in Inventory) {
                Debug.Log($"res add ${resource.ResourceType}");
                if (resource.ResourceType != resourceType) continue;

                resource.Quantity += quantity;
                if (State is FoundationState foundationState)
                {
                    foundationState.CheckRequiredResources(this);
                }
                return;
                
            }
            throw new Exception($"trying to add resource that is not in the inputType ${resourceType}");
        }

        public void RemoveResource(ResourceType resourceType, int quantity)
        {
            foreach (Resource resource in Inventory)
            {
                if (resource.ResourceType == resourceType)
                {
                    resource.Quantity -= quantity;
                    return;
                }
            }
            throw new Exception("trying to remove resource, but no resource in output has quantity > 0");
        }

        public bool CheckHasOutput()
        {
            foreach (Resource resource in Inventory)
            {
                if (resource.Quantity > 0)
                {
                    return true;
                }
            }
            return false;
        }

        protected void CreateJob(Job job)
        {
            _jobQueue.AddJob(job);
        }

        public Resource[] GetInventory() {
            return Inventory;
        }

        protected void TransmuteResource(Resource[] input, Resource[] output) {
            foreach (Resource resource in input)
            {
                RemoveResource(resource.ResourceType, resource.Quantity);
            }

            foreach (Resource resource in output)
            {
                AddResource(resource.ResourceType, resource.Quantity);
            }
        }

        public int GetCapacity() {
            return Capacity;
        }
    }
}