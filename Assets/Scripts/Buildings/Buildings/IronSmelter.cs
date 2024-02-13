namespace MyRTSGame.Model
{
    public class IronSmelter : Building
    {
        //Constructor
        public IronSmelter()
        {
            BuildingType = BuildingType.IronSmelter;
        }

        protected override void Start()
        {            
            base.Start();
            
            ResourceType[] resourceTypes = { ResourceType.IronOre, ResourceType.Iron };
            var resourceQuantities = new int[resourceTypes.Length];
            InventoryWhenCompleted = InitInventory(resourceTypes, resourceQuantities);
            InputTypesWhenCompleted = new[] { ResourceType.Lumber };
            HasInput = true;
        }
        
        public override void StartResourceCreationCoroutine()
        {
            Resource[] input = { new() { ResourceType = ResourceType.IronOre, Quantity = 1 }, new() { ResourceType = ResourceType.Coal, Quantity = 1 } };
            Resource[] output = { new() { ResourceType = ResourceType.Iron, Quantity = 1 } };
            StartCoroutine(buildingController.CreateOutputFromInput(this, 10, input, output));
        }
    }
}