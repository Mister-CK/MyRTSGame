namespace MyRTSGame.Model
{
    public class IronSmelter : ProductionBuilding
    {
        //Constructor
        public IronSmelter()
        {
            BuildingType = BuildingType.IronSmelter;
        }

        protected override void Start()
        {            
            base.Start();
            
            ResourceType[] resourceTypes = { ResourceType.IronOre, ResourceType.Coal, ResourceType.Iron };
            var resourceQuantities = new int[resourceTypes.Length];
            InventoryWhenCompleted = InitInventory(resourceTypes);
            InputTypesWhenCompleted = new[] { ResourceType.IronOre, ResourceType.Coal};
            OutputTypesWhenCompleted = new[] { ResourceType.Iron};
            HasInput = true;
        }
        
        public override void StartResourceCreationCoroutine()
        {
            Resource[] input = { new() { ResourceType = ResourceType.IronOre, Quantity = 1 }, new() { ResourceType = ResourceType.Coal, Quantity = 1 } };
            Resource[] output = { new() { ResourceType = ResourceType.Iron, Quantity = 1 } };
            StartCoroutine(CreateOutputFromInput(10, input, output));
        }
    }
}