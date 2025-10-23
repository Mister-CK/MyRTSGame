using Enums;
using MyRTSGame.Model;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Buildings.Model.BuildingGroups
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
                    ModifyInventory(resource.ResourceType, data => data.Current -= resource.Quantity);
                }

                ModifyInventory(productionJob.Output.ResourceType,
                    data => data.Current -= productionJob.Output.Quantity);
                
                BuildingService.CreateJobNeededEvent(JobType.VillagerJob, null, this, productionJob.Output.ResourceType, null);
            }
        }
    }
}