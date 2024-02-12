namespace MyRTSGame.Model
{
    public class GoldMine : Building
    {
        //Constructor
        public GoldMine()
        {
            BuildingType = BuildingType.GoldMine;
        }

        protected override void Start()
        {
            base.Start();

            ResourceType[] resourceTypes = { ResourceType.GoldOre };
            var resourceQuantities = new int[resourceTypes.Length];
            InventoryWhenCompleted = InitInventory(resourceTypes, resourceQuantities);
        }

        public override void StartResourceCreationCoroutine()
        {
            StartCoroutine(buildingController.CreateResource(this, 15, ResourceType.GoldOre));
        }
    }
}