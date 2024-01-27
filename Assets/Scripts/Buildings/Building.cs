using System;
using UnityEngine;
using MyRTSGame.Interface;
namespace MyRTSGame.Model
{

    public abstract class Building : MonoBehaviour
    {
        protected IBuildingState State;
        public BuildingType BuildingType { get; protected set; }
        protected ResourceType[] InputTypes = Array.Empty<ResourceType>();
        protected Resource[] Inventory;
        protected int Capacity = 999;
        
        private GameObject _buildingObject;
        private JobQueue _jobQueue;

        protected virtual void Start()
        {
            State = new FoundationState(BuildingType);
        }

        public void OnClick()
        {
            State.OnClick(this);
        }

        public void SetState(IBuildingState newState)
        {
            State = newState;
            State.SetObject(this);
        }

        public void SetObject(GameObject newObject)
        {
            if (_buildingObject != null)
            {
                Destroy(_buildingObject);
            }
            _buildingObject = Instantiate(newObject, transform.position, Quaternion.identity, transform);
        }

        void Awake()
        {
            _jobQueue = JobQueue.GetInstance();
            ResourceType[] reourceTypes = new ResourceType[0];
            int[] resourceQuantities = new int[0];
            InputTypes = new ResourceType[0];
            Inventory = InitInventory(reourceTypes, resourceQuantities);
        }

        public virtual bool IsWarehouse
        {
            get { return false; }
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
            foreach(Resource resource in Inventory) {
                if (resource.ResourceType == resourceType) {
                    resource.Quantity += quantity;
                    return;
                }
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

        public ResourceType[] GetInputTypes()
        {
            return InputTypes;
        }

        public int GetCapacity() {
            return Capacity;
        }
    }
}