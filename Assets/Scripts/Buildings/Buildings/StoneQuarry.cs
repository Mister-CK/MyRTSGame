namespace MyRTSGame.Model
{
    public class StoneQuarry : Building
    {
        //Constructor
        public StoneQuarry()
        {
            BuildingType = BuildingType.StoneQuarry;
        }

        protected override void Start()
        {
            base.Start();

            ResourceType[] resourceTypes = { ResourceType.Stone };
            int[] resourceQuantities = { 0 };
            Inventory = InitInventory(resourceTypes, resourceQuantities);
            InventoryWhenCompleted = InitInventory(resourceTypes, resourceQuantities);
        }

        public override void StartResourceCreationCoroutine()
        {
            StartCoroutine(buildingController.CreateResource(this, 15, ResourceType.Stone));
        }
    }
}