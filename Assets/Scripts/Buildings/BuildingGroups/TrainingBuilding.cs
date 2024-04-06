using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyRTSGame.Model
{
    public class TrainingBuilding : Building
    {
        public List<TrainingJob> TrainingJobs;
        public List<UnitType> trainableUnits;
        public void AddTrainingJob(UnitType unitType)
        {
            TrainingJobs.First(job => job.UnitType == unitType).Quantity++;
        }
        
        public void RemoveTrainingJob(UnitType unitType)
        {
            TrainingJobs.First(job => job.UnitType == unitType).Quantity--;
        }
         
        private TrainingJob GetTrainingJob()
        {
            foreach (var trainingJob in TrainingJobs)
            {
                if (trainingJob.Quantity <= 0) continue;
                if (!CheckIfRequiredResourceAreAvailable(trainingJob.Input)) continue;
                
                trainingJob.Quantity--;
                return trainingJob;
            }

            return null;
        }
        
        private bool CheckIfRequiredResourceAreAvailable(IEnumerable<Resource> inputTypes)
        {
            return inputTypes.All(resource => 
                Inventory.FirstOrDefault(res => res.ResourceType == resource.ResourceType)?.Quantity > resource.Quantity);
        }
        
        protected IEnumerator TrainUnitFromQueue(int timeInSeconds)
        {
            while (true)
            {
                yield return new WaitForSeconds(timeInSeconds);
                
                var trainingJob = GetTrainingJob();
                if (trainingJob == null) continue;
                
                foreach (var resource in trainingJob.Input)
                {
                    RemoveResource(resource.ResourceType, resource.Quantity);
                }
                
                //Create new Unit of type trainingJob.Unit
            }
        }
    }
}