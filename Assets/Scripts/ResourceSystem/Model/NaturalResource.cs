using System;
using System.Collections.Generic;
using MyRTSGame.Model.ResourceSystem.Controller;
using UnityEngine;

namespace MyRTSGame.Model.ResourceSystem.Model
{
    public class NaturalResource: MonoBehaviour, IDestination, ISelectable, IInventory
    {
        public Dictionary<ResourceType, InventoryData> Inventory { get; set; }
        protected ResourceType ResourceType;
        public BoxCollider BCollider { get; private set; }
        protected ResourceController ResourceController;
        private List<CollectResourceJob> _collectResourceJobs = new List<CollectResourceJob>();

        protected virtual void Start()
        {
            ResourceController = ResourceController.Instance;
        }
        
        public ResourceType GetResourceType()
        {
            return ResourceType;
        }
        public void SetResourceType(ResourceType resourceType)
        {
            ResourceType = resourceType;
        }
        
        public Vector3 GetPosition()
        {
            return transform.position;
        }
        
        public void AddJobToDestination(Job job)
        {
            if (job is not CollectResourceJob collectResourceJob) return;
            _collectResourceJobs.Add(collectResourceJob);
        }
        
        
        public void RemoveResource(ResourceType resourceType, int quantity)
        {
            Inventory[resourceType].Outgoing -= quantity;
            Inventory[resourceType].Current -= quantity;
            if (IsInventoryEmpty())
            {
                Destroy(gameObject); //this doesn't work well, the object is destroyed but it remains in the UI.
            }
        }
        
        public virtual void AddResource(ResourceType resourceType, int quantity)
        {
            Inventory[resourceType].Incoming -= quantity;
            Inventory[resourceType].Current += quantity;
        }
        
        public Dictionary<ResourceType, InventoryData> GetInventory()
        {
            return Inventory;
        }
        
        public void ModifyInventory(ResourceType resourceType, Action<InventoryData> modifyAction)
        {
            if (Inventory.ContainsKey(resourceType))
            {
                modifyAction(Inventory[resourceType]);
            }
            else
            {
                throw new ArgumentException($"The inventory does not contain the resource type: {resourceType}");
            }
        }
        private bool IsInventoryEmpty()
        {
            return Inventory[ResourceType].Current <= 0;
        }
    }
}