namespace MyRTSGame.Model
{

    public class Warehouse : Building
    {
        //Constructor
        public Warehouse() {
            BuildingType = BuildingType.Warehouse;
        }

        protected override void Start()
        {
            State = new PlacingState(BuildingType);
            ResourceType[] resourceTypes = new ResourceType[] { ResourceType.Stone, ResourceType.Lumber, ResourceType.Wood };
            int[] resourceQuantities = new int[] { 0, 0, 0};
            Inventory = InitInventory(resourceTypes, resourceQuantities);
            InputTypes = resourceTypes;
        }

        public override bool IsWarehouse
        {
            get { return true; }
        }
    }
}