namespace MyRTSGame.Model
{
    public class PigFarm : ProductionBuilding
    {
        //Constructor
        public PigFarm()
        {
            BuildingType = BuildingType.PigFarm;
        }

        protected override void Start()
        {            
            base.Start();
            
            ResourceType[] resourceTypes = { ResourceType.Wheat, ResourceType.Hides, ResourceType.Pork };
            var resourceQuantities = new int[resourceTypes.Length];
            InventoryWhenCompleted = InitInventory(resourceTypes, resourceQuantities);
            InputTypesWhenCompleted = new[] { ResourceType.Wheat};
            OutputTypesWhenCompleted = new[] { ResourceType.Pork, ResourceType.Hides};
            HasInput = true;
        }
        
        public override void StartResourceCreationCoroutine()
        {
            Resource[] input = { new() { ResourceType = ResourceType.Wheat, Quantity = 1 } };
            Resource[] output = { new() { ResourceType = ResourceType.Pork, Quantity = 1 }, new() { ResourceType = ResourceType.Hides, Quantity = 1}};
            StartCoroutine(CreateOutputFromInput(10, input, output));
        }
    }
}