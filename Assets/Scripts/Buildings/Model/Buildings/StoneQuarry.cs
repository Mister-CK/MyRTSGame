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
            OutputTypesWhenCompleted = new[] { ResourceType.Stone };
        }

        public override void StartResourceCreationCoroutine()
        {
            StartCoroutine(CreateResource( 15, ResourceType.Stone));
        }
    }
}