namespace MyRTSGame.Model
{
    public class SawMill : ProductionBuilding
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
            var resourceQuantities = new int[resourceTypes.Length];
            InventoryWhenCompleted = InitInventory(resourceTypes, resourceQuantities);
            InputTypesWhenCompleted = new[] { ResourceType.Lumber };
            OutputTypesWhenCompleted = new[] { ResourceType.Wood };
            HasInput = true;
        }
        
        public override void StartResourceCreationCoroutine()
        {
            Resource[] input = { new() { ResourceType = ResourceType.Lumber, Quantity = 1 } };
            Resource[] output = { new() { ResourceType = ResourceType.Wood, Quantity = 1 } };
            StartCoroutine(CreateOutputFromInput(10, input, output));
        }
    }
}