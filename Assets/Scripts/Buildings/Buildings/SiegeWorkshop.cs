using System.Collections.Generic;

namespace MyRTSGame.Model
{
    public class SiegeWorkshop : TrainingBuilding
    {
        private TrainingJob VillagerJob = new TrainingJob
        {
            Input = new Resource[]
            {
                new Resource { ResourceType = ResourceType.Gold, Quantity = 1 }
            },
            UnitType = UnitType.Villager,
            Quantity = 0
        };
        
        TrainingJob BuilderJob = new TrainingJob
        {
            Input = new Resource[]
            {
                new Resource { ResourceType = ResourceType.Gold, Quantity = 1 }
            },
            UnitType = UnitType.Builder,
            Quantity = 0
        };
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
            TrainingJobs = new List<TrainingJob>() {VillagerJob, BuilderJob};
            HasInput = true;
        }
    }
}