using System.Collections.Generic;

namespace MyRTSGame.Model
{
    public class School : TrainingBuilding
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
            TrainingJobs = new List<TrainingJob>() {VillagerJob, BuilderJob};
            HasInput = true;
        }
        
        public override void StartResourceCreationCoroutine()
        {
            StartCoroutine(TrainUnitFromQueue(5));
        }
    }
}