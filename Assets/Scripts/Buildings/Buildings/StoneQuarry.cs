namespace MyRTSGame.Model
{
    public class StoneQuarry : ResourceBuilding
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
            var resourceQuantities = new int[resourceTypes.Length];
            InventoryWhenCompleted = InitInventory(resourceTypes, resourceQuantities);
        }

        public override void StartResourceCreationCoroutine()
        {
            StartCoroutine(buildingController.CreateResource(this, 15, ResourceType.Stone));
        }
    }
}