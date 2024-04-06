using System.Collections.Generic;

namespace MyRTSGame.Model
{
    public class SiegeWorkshop : TrainingBuilding
    {
        //Constructor
        public SiegeWorkshop()
        {
            BuildingType = BuildingType.SiegeWorkshop;
        }

        protected override void Start()
        {            
            base.Start();
            
            ResourceType[] resourceTypes = {ResourceType.Wood, ResourceType.Iron};
            var resourceQuantities = new int[resourceTypes.Length];
            InventoryWhenCompleted = InitInventory(resourceTypes, resourceQuantities);
            InputTypesWhenCompleted = new[] {ResourceType.Wood, ResourceType.Iron, };
            //TODO: should have siege weapons as trainingJobs, instead of villagers and builders
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