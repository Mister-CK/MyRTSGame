namespace MyRTSGame.Model
{
    public class GoldSmelter : ProductionBuilding
    {
        //Constructor
        public GoldSmelter()
        {
            BuildingType = BuildingType.GoldSmelter;
        }

        protected override void Start()
        {            
            base.Start();
            
            ResourceType[] resourceTypes = { ResourceType.GoldOre, ResourceType.Coal, ResourceType.Gold };
            var resourceQuantities = new int[resourceTypes.Length];
            InventoryWhenCompleted = InitInventory(resourceTypes);
            InputTypesWhenCompleted = new[] { ResourceType.GoldOre, ResourceType.Coal};
            OutputTypesWhenCompleted = new[] { ResourceType.Gold};
            HasInput = true;
        }
        
        public override void StartResourceCreationCoroutine()
        {
            Resource[] input = { new() { ResourceType = ResourceType.GoldOre, Quantity = 1 }, new() { ResourceType = ResourceType.Coal, Quantity = 1 } };
            Resource[] output = { new() { ResourceType = ResourceType.Gold, Quantity = 1 } };
            StartCoroutine(CreateOutputFromInput(10, input, output));
        }
    }
}