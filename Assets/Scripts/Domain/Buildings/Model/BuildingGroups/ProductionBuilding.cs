using Enums;
using Domain;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Buildings.Model.BuildingGroups
{
    public abstract class ProductionBuilding : Building
    {
        private void TransmuteResource(IEnumerable<Resource> input, IEnumerable<Resource> output)
        {
            foreach (var resource in input) ModifyInventory(resource.ResourceType, data => data.Current -= resource.Quantity);

            foreach (var resource in output) ModifyInventory(resource.ResourceType,data => data.Current += resource.Quantity);
        }
        
        protected IEnumerator CreateOutputFromInput(int intervalInSeconds, Resource[] input, Resource[] output)
        {
            while (true)
            {
                yield return new WaitForSeconds(intervalInSeconds);

                // Check if all required resources are present with quantity > 0
                var hasRequiredResources = input.All(resource => 
                    Inventory.FirstOrDefault(res => res.Key == resource.ResourceType).Value.Current > 0);

                // Check if all output resources have quantity < capacity
                var isFull = output.All(resource => 
                    Inventory.FirstOrDefault(res => res.Key == resource.ResourceType).Value.Current >= Capacity);

                if (!hasRequiredResources || isFull) continue;
                
                TransmuteResource(input, output);
                foreach (var resource in output)
                {
                    BuildingService.CreateJobNeededEvent(JobType.VillagerJob, null, this, resource.ResourceType, null);
                }
            }
        }
    }
}