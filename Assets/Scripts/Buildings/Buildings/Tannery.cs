namespace MyRTSGame.Model
{
    public class Tannery : Building
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
            InventoryWhenCompleted = InitInventory(resourceTypes, resourceQuantities);
            InputTypesWhenCompleted = new[] { ResourceType.Hides};
            HasInput = true;
        }
        
        public override void StartResourceCreationCoroutine()
        {
            Resource[] input = { new() { ResourceType = ResourceType.Hides, Quantity = 1 } };
            Resource[] output = { new() { ResourceType = ResourceType.Leather, Quantity = 1 } };
            StartCoroutine(buildingController.CreateOutputFromInput(this, 10, input, output));
        }
    }
}