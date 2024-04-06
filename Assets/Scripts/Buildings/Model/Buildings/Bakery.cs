namespace MyRTSGame.Model
{
    public class Bakery : ProductionBuilding
    {
        //Constructor
        public Bakery()
        {
            BuildingType = BuildingType.Bakery;
        }

        protected override void Start()
        {
            base.Start();

            ResourceType[] resourceTypes = { ResourceType.Flour, ResourceType.Bread };
            var resourceQuantities = new int[resourceTypes.Length];
            InventoryWhenCompleted = InitInventory(resourceTypes, resourceQuantities);
            InputTypesWhenCompleted = new[] { ResourceType.Flour };
            OutputTypesWhenCompleted = new[] { ResourceType.Bread };
            HasInput = true;
        }

        public override void StartResourceCreationCoroutine()
        {
            Resource[] input = { new() { ResourceType = ResourceType.Flour, Quantity = 1 } };
            Resource[] output = { new() { ResourceType = ResourceType.Bread, Quantity = 1 } };
            StartCoroutine(CreateOutputFromInput(10, input, output));
        }
    }
}