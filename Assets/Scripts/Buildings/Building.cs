using System;
using UnityEngine;
using MyRTSGame.Interface;
namespace MyRTSGame.Model
{

    public abstract class Building : MonoBehaviour
    {
        protected IBuildingState state;
        protected GameObject buildingObject;
        protected BuildingType buildingType;
        protected Resource[] inventory;
        protected JobQueue jobQueue;
        protected ResourceType[] _inputTypes = new ResourceType[0];
        protected int _capacity = 999;

        protected virtual void Start()
        {
            state = new FoundationState(buildingType);
        }

        public void OnClick()
        {
            state.OnClick(this);
        }

        public void SetState(IBuildingState newState)
        {
            state = newState;
            state.SetObject(this);
        }

        public void SetObject(GameObject newObject)
        {
            if (buildingObject != null)
            {
                Destroy(buildingObject);
            }
            buildingObject = Instantiate(newObject, transform.position, Quaternion.identity, transform);
        }

        void Awake()
        {
            jobQueue = JobQueue.GetInstance();
            ResourceType[] reourceTypes = new ResourceType[0];
            int[] resourceQuantities = new int[0];
            _inputTypes = new ResourceType[0];
            inventory = InitInventory(reourceTypes, resourceQuantities);
        }

        public virtual bool IsWarehouse
        {
            get { return false; }
        }

        public Resource[] InitInventory(ResourceType[] types, int[] quantities)
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
            foreach(Resource resource in inventory) {
                if (resource.ResourceType == resourceType) {
                    resource.Quantity += quantity;
                    return;
                }
            }
            throw new Exception($"trying to add resource that is not in the inputType ${resourceType}");
        }

        public void RemoveResource(ResourceType resourceType, int quantity)
        {
            foreach (Resource resource in inventory)
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
            foreach (Resource resource in inventory)
            {
                if (resource.Quantity > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public void CreateJob(Job job)
        {
            jobQueue.AddJob(job);
        }

        public Resource[] GetInventory() {
            return this.inventory;
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
            return this._inputTypes;
        }

        public int GetCapacity() {
            return this._capacity;
        }

        public BuildingType GetBuildingType()
        {
            return this.buildingType;
        }


        protected void SetBuildingType(BuildingType newBuildingtype)
        {
            this.buildingType = newBuildingtype;
        }
    }
}