using System;
using System.Collections.Generic;

namespace MyRTSGame.Model
{
    public interface IInventory
    {
        public void ModifyInventory(ResourceType resourceType, Action<InventoryData> modifyAction);
        public void RemoveResource(ResourceType resourceType, int quantity);
        public void AddResource(ResourceType resourceType, int quantity);

        public Dictionary<ResourceType, InventoryData> GetInventory();  
        public void SetInventory(Dictionary<ResourceType, InventoryData> inventory);
    }
    
    public static class InventoryHelper
    {
        public static Dictionary<ResourceType, InventoryData> InitInventory(IEnumerable<ResourceType> resTypes)
        {
            var inventory = new Dictionary<ResourceType, InventoryData>();

            foreach (var resType in resTypes)
            {
                inventory[resType] = new InventoryData() { InJob = 0, Current = 0, Outgoing = 0, Incoming = 0};
            }

            return inventory;
        }
    }
}