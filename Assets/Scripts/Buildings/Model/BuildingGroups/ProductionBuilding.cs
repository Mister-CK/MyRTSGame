using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyRTSGame.Model
{
    public abstract class ProductionBuilding : Building
    {
        private void TransmuteResource(IEnumerable<Resource> input, IEnumerable<Resource> output)
        {
            foreach (var resource in input) RemoveResource(resource.ResourceType, resource.Quantity);

            foreach (var resource in output) AddResource(resource.ResourceType, resource.Quantity);
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
                    buildingController.CreateJobNeededEvent(JobType.VillagerJob, null, this, resource.ResourceType);
                }
            }
        }
    }
}