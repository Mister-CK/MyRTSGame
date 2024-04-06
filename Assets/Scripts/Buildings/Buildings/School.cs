using System.Collections.Generic;

namespace MyRTSGame.Model
{
    public class School : TrainingBuilding
    {
        //Constructor
        public School()
        {
            BuildingType = BuildingType.School;
        }

        protected override void Start()
        {            
            base.Start();
            
            ResourceType[] resourceTypes = { ResourceType.Gold};
            var resourceQuantities = new int[resourceTypes.Length];
            InventoryWhenCompleted = InitInventory(resourceTypes, resourceQuantities);
            InputTypesWhenCompleted = new[] { ResourceType.Gold };
            trainableUnits =  new List<UnitType>() {UnitType.Villager, UnitType.Builder};
            TrainingJobs = GetTrainingJobsForUnitTypes(trainableUnits);
            HasInput = true;
        }
        
        public override void StartResourceCreationCoroutine()
        {
            StartCoroutine(TrainUnitFromQueue(5));
        }
    }
}