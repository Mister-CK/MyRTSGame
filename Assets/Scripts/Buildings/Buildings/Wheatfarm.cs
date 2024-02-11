namespace MyRTSGame.Model
{
    public class WheatFarm: Building
    {
        public WheatFarm()
        {
            BuildingType = BuildingType.WheatFarm;
        }

        protected override void Start()
        {
            base.Start();

            ResourceType[] resourceTypes = { ResourceType.Wheat };
            var resourceQuantities = new int[resourceTypes.Length];
            Inventory = InitInventory(resourceTypes, resourceQuantities);
            InventoryWhenCompleted = InitInventory(resourceTypes, resourceQuantities);
        }

        public override void StartResourceCreationCoroutine()
        {
            StartCoroutine(buildingController.CreateResource(this, 15, ResourceType.Wheat));
        }
    }
}