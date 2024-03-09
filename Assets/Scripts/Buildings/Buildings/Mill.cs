namespace MyRTSGame.Model
{
    public class Mill : ProductionBuilding
    {
        //Constructor
        public Mill()
        {
            BuildingType = BuildingType.Mill;
        }

        protected override void Start()
        {
            base.Start();

            ResourceType[] resourceTypes = { ResourceType.Wheat, ResourceType.Flour };
            var resourceQuantities = new int[resourceTypes.Length];
            InventoryWhenCompleted = InitInventory(resourceTypes, resourceQuantities);
            InputTypesWhenCompleted = new[] { ResourceType.Wheat };
            HasInput = true;
        }

        public override void StartResourceCreationCoroutine()
        {
            Resource[] input = { new() { ResourceType = ResourceType.Wheat, Quantity = 1 } };
            Resource[] output = { new() { ResourceType = ResourceType.Flour, Quantity = 1 } };
            StartCoroutine(buildingController.CreateOutputFromInput(this, 10, input, output));
        }
    }
}