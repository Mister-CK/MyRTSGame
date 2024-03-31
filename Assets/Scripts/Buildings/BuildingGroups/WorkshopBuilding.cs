using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyRTSGame.Model
{
    public abstract class WorkshopBuilding : Building
    {
        protected List<ProductionJob> ProductionQueue;
        
        protected void AddProductionJobToQueue(ProductionJob productionJob)
        {
            ProductionQueue.Add(productionJob);
        }
        
        private ProductionJob GetFirstProductionJob()
        {

            var productionJob = ProductionQueue.FirstOrDefault();
            if (productionJob == null) return null;
            if (!CheckIfRequiredResourceAreAvailable(productionJob.Input)) return null;
            ProductionQueue.RemoveAt(0);
            
            return productionJob;
        }
        
        private bool CheckIfRequiredResourceAreAvailable(Resource[] inputTypes)
        {
            return inputTypes.All(resource => 
                Inventory.FirstOrDefault(res => res.ResourceType == resource.ResourceType)?.Quantity > resource.Quantity);
        }
        
        protected IEnumerator CreateResourceFromQueue(int timeInSeconds)
        {
            while (true)
            {
                yield return new WaitForSeconds(timeInSeconds);
                
                var productionJob = GetFirstProductionJob();
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