namespace MyRTSGame.Model
{
    public class Tannery : ProductionBuilding
    {
        //Constructor
        public Tannery()
        {
            BuildingType = BuildingType.Tannery;
        }

        protected override void Start()
        {            
            base.Start();
            
            ResourceType[] resourceTypes = { ResourceType.Hides, ResourceType.Leather };
            var resourceQuantities = new int[resourceTypes.Length];
            InventoryWhenCompleted = InitInventory(resourceTypes);
            InputTypesWhenCompleted = new[] { ResourceType.Hides};
            OutputTypesWhenCompleted = new[] { ResourceType.Leather};

            HasInput = true;
        }
        
        public override void StartResourceCreationCoroutine()
        {
            Resource[] input = { new() { ResourceType = ResourceType.Hides, Quantity = 1 } };
            Resource[] output = { new() { ResourceType = ResourceType.Leather, Quantity = 1 } };
            StartCoroutine(CreateOutputFromInput(10, input, output));
        }
    }
}