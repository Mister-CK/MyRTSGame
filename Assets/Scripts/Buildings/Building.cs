using System;
using Unity.VisualScripting;
using UnityEngine;

namespace MyRTSGame.Model
{
    public abstract class Building : MonoBehaviour, ISelectable
    {
        public bool HasInput;
        private GameObject _buildingObject;
        private JobQueue _jobQueue;
        private SelectionManager _selectionManager;
        protected int Capacity = 999;
        protected Resource[] Inventory;
        protected IBuildingState State;
        public Material Material { get; set; }
        public BuildingType BuildingType { get; set; }
        public ResourceType[] InputTypes { get; set; }
        public ResourceType[] InputTypesWhenCompleted { get; set; }
        public BoxCollider BCollider { get; private set; }
        private BuildingList _buildingList;
        private void Awake()
        {
            BCollider = this.AddComponent<BoxCollider>();
            BCollider.size = new Vector3(3, 3, 3);
            _jobQueue = JobQueue.GetInstance();
            var resourceTypes = new ResourceType[0];
            var resourceQuantities = new int[0];
            InputTypes = new ResourceType[0];
            Inventory = InitInventory(resourceTypes, resourceQuantities);
        }

        protected virtual void Start()
        {
            State = new PlacingState(BuildingType);
        }

        private void Update()
        {
            _buildingList = BuildingList.Instance; // should be in start, but start is virtual here
            _selectionManager = SelectionManager.Instance; // should be in start, but start is virtual here

            if (State is PlacingState placingState) placingState.CheckOverlap(this);
        }

        public void OnMouseDown()
        {
            OnClick();
        }

        public abstract Resource[] GetRequiredResources();

        private void OnClick()
        {
            if (GetState() is FoundationState foundationState) foundationState.OnClick(this);

            _selectionManager.SelectObject(this);
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

        protected static Resource[] InitInventory(ResourceType[] types, int[] quantities)
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
            Debug.Log("AddResource");
            foreach (var resource in Inventory)
            {
                Debug.Log($"res add ${resource.ResourceType}");
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

        // public bool CheckHasOutput()
        // {
        //     foreach (Resource resource in Inventory)
        //     {
        //         if (resource.Quantity > 0)
        //         {
        //             return true;
        //         }
        //     }
        //     return false;
        // }

        protected void CreateJob(Job job)
        {
            job.Destination = FindDestinationForJob(job);
            _jobQueue.AddJob(job);
        }
        
        private Building FindDestinationForJob(Job job)
        {
            var buildings = _buildingList.GetBuildings();
            Building destination = null;
            Building warehouse = null;
            var resourceType = job.ResourceType;

            foreach (var building in buildings)
            {
                if (building.BuildingType == BuildingType.Warehouse)
                {
                    warehouse = building;
                    continue;
                }

                var inputTypes = building.InputTypes;
                var inventory = building.GetInventory();

                if (Array.IndexOf(inputTypes, resourceType) == -1) continue;
                var resourceInInventory = Array.Find(inventory, res => res.ResourceType == resourceType);
                if (resourceInInventory != null && resourceInInventory.Quantity >= building.GetCapacity()) continue;
                destination = building;
                break;
            }

            // If no suitable building is found, set destination to Warehouse
            if (destination == null)
            {
                destination = warehouse;
                if (destination == null) throw new Exception("No Destination found");
            }

            return destination;
        }

        public Resource[] GetInventory()
        {
            return Inventory;
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