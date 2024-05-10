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
                Inventory.FirstOrDefault(res => res.Key == resource.ResourceType).Value.Current > resource.Quantity);
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
                    ModifyInventory(resource.ResourceType, data => data.Current -= resource.Quantity);
                }
                
                BuildingController.CreateNewUnitEvent(this, trainingJob.UnitType);
            }
        }
        
        protected List<TrainingJob> GetTrainingJobsForUnitTypes(IEnumerable<UnitType> unitTypes)
        {
            List<TrainingJob> trainingJobs = new List<TrainingJob>();
            foreach(var unitType in unitTypes)
            {
                switch(unitType)
                {
                    case UnitType.Villager:
                        trainingJobs.Add(_villagerJob);
                        break;
                    case UnitType.Builder:
                        trainingJobs.Add(_builderJob);
                        break;
                    case UnitType.StoneMiner:
                        trainingJobs.Add(_stoneMiner);
                        break;
                    case UnitType.LumberJack:
                        trainingJobs.Add(_lumberjackJob);
                        break;
                    case UnitType.Farmer:
                        trainingJobs.Add(_farmer);
                        break;
                    default:
                        throw new System.ArgumentOutOfRangeException(unitType.ToString());
                }
            }
            return trainingJobs;
        }
        
        private readonly TrainingJob _villagerJob = new TrainingJob
        {
            Input = new Resource[]
            {
                new Resource { ResourceType = ResourceType.Gold, Quantity = 2 }
            },
            UnitType = UnitType.Villager,
            Quantity = 0
        };
        
        private readonly TrainingJob _builderJob = new TrainingJob
        {
            Input = new Resource[]
            {
                new Resource { ResourceType = ResourceType.Gold, Quantity = 2 }
            },
            UnitType = UnitType.Builder,
            Quantity = 0
        };
        
        private readonly TrainingJob _stoneMiner = new TrainingJob
        {
            Input = new Resource[]
            {
                new Resource { ResourceType = ResourceType.Gold, Quantity = 2 }
            },
            UnitType = UnitType.StoneMiner,
            Quantity = 0
        };
        
        private readonly TrainingJob _lumberjackJob = new TrainingJob
        {
            Input = new Resource[]
            {
                new Resource { ResourceType = ResourceType.Gold, Quantity = 2 }
            },
            UnitType = UnitType.LumberJack,
            Quantity = 0
        };
        private readonly TrainingJob _farmer = new TrainingJob
        {
            Input = new Resource[]
            {
                new Resource { ResourceType = ResourceType.Gold, Quantity = 2 }
            },
            UnitType = UnitType.Farmer,
            Quantity = 0
        };
    }
}