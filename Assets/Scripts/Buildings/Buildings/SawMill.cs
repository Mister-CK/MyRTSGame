namespace MyRTSGame.Model
{
    public class SawMill : Building
    {
        //Constructor
        public SawMill()
        {
            BuildingType = BuildingType.SawMill;
        }

        protected override void Start()
        {            
            base.Start();
            
            ResourceType[] resourceTypes = { ResourceType.Lumber, ResourceType.Wood };
            int[] resourceQuantities = { 0, 0 };
            Inventory = InitInventory(resourceTypes, resourceQuantities);
            InventoryWhenCompleted = InitInventory(resourceTypes, resourceQuantities);
            InputTypesWhenCompleted = new[] { ResourceType.Lumber };
            HasInput = true;
        }
        
        public override void StartResourceCreationCoroutine()
        {
            Resource[] input = { new() { ResourceType = ResourceType.Lumber, Quantity = 1 } };
            Resource[] output = { new() { ResourceType = ResourceType.Wood, Quantity = 1 } };
            StartCoroutine(BuildingController.CreateOutputFromInput(10, input, output));
        }
    }
}