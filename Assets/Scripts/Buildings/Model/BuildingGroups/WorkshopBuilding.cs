using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyRTSGame.Model
{
    public abstract class WorkshopBuilding : Building
    {
        public List<ProductionJob> ProductionJobs; 

        public void AddProductionJob(ResourceType resourceType)
        {
            ProductionJobs.First(job => job.Output.ResourceType == resourceType).Quantity++;
        }
        
        public void RemoveProductionJob(ResourceType resourceType)
        {
            ProductionJobs.First(job => job.Output.ResourceType == resourceType).Quantity--;
        }
         
        private ProductionJob GetProductionJob()
        {
            foreach (var productionJob in ProductionJobs)
            {
                if (productionJob.Quantity <= 0) continue;
                if (!CheckIfRequiredResourceAreAvailable(productionJob.Input)) continue;
                
                productionJob.Quantity--;
                return productionJob;
            }

            return null;
        }
        
        
        private bool CheckIfRequiredResourceAreAvailable(IEnumerable<Resource> inputTypes)
        {
            return inputTypes.All(resource => 
                Inventory.FirstOrDefault(res => res.Key == resource.ResourceType).Value.Current > resource.Quantity);
        }
        
        protected IEnumerator CreateResourceFromQueue(int timeInSeconds)
        {
            while (true)
            {
                yield return new WaitForSeconds(timeInSeconds);
                
                var productionJob = GetProductionJob();
                if (productionJob == null) continue;
                
                foreach (var resource in productionJob.Input)
                {
                    RemoveResource(resource.ResourceType, resource.Quantity);
                }
                AddResource(productionJob.Output.ResourceType, productionJob.Output.Quantity);
                
                buildingController.CreateVillagerJobNeededEvent(this, productionJob.Output.ResourceType);
            }
        }
    }
}