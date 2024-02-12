namespace MyRTSGame.Model
{
    public class Butcher : Building
    {
        //Constructor
        public Butcher()
        {
            BuildingType = BuildingType.Butcher;
        }

        protected override void Start()
        {            
            base.Start();
            
            ResourceType[] resourceTypes = { ResourceType.Pork, ResourceType.Sausage };
            var resourceQuantities = new int[resourceTypes.Length];
            InventoryWhenCompleted = InitInventory(resourceTypes, resourceQuantities);
            InputTypesWhenCompleted = new[] { ResourceType.Pork };
            HasInput = true;
        }
        
        public override void StartResourceCreationCoroutine()
        {
            Resource[] input = { new() { ResourceType = ResourceType.Pork, Quantity = 1 } };
            Resource[] output = { new() { ResourceType = ResourceType.Sausage, Quantity = 1 } };
            StartCoroutine(buildingController.CreateOutputFromInput(this, 10, input, output));
        }
    }
}