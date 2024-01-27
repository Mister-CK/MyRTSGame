namespace MyRTSGame.Model
{

    public class Warehouse : Building
    {
        //Constructor
        public Warehouse() {
            SetBuildingType(BuildingType.Warehouse);
        }

        protected override void Start()
        {
            SetBuildingType(BuildingType.Warehouse);
            state = new FoundationState(buildingType);
            ResourceType[] resourceTypes = new ResourceType[] { ResourceType.Stone, ResourceType.Lumber, ResourceType.Wood };
            int[] resourceQuantities = new int[] { 0, 0, 0};
            inventory = InitInventory(resourceTypes, resourceQuantities);
            _inputTypes = resourceTypes;
        }

        public override bool IsWarehouse
        {
            get { return true; }
        }
    }
}