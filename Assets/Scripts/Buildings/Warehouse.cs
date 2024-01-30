namespace MyRTSGame.Model
{

    public class Warehouse : Building
    {
        //Constructor
        public Warehouse() {
            BuildingType = BuildingType.Warehouse;
        }
        
        public override Resource[] GetRequiredResources()
        {
            return new Resource[] { new Resource() { ResourceType = ResourceType.Lumber, Quantity = 1}, new Resource() { ResourceType = ResourceType.Stone, Quantity = 0 } };
        }

        protected override void Start()
        {
            State = new PlacingState(BuildingType);
            ResourceType[] resourceTypes = new ResourceType[] { ResourceType.Stone, ResourceType.Lumber, ResourceType.Wood };
            int[] resourceQuantities = new int[] { 0, 0, 0};
            Inventory = InitInventory(resourceTypes, resourceQuantities);
            InputTypesWhenCompleted = resourceTypes;
            HasInput = true;
        }

        public override bool IsWarehouse
        {
            get { return true; }
        }
    }
}