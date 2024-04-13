namespace MyRTSGame.Model
{
    public class Vineyard : ResourceBuilding
    {
        //Constructor
        public Vineyard()
        {
            BuildingType = BuildingType.Vineyard;
        }

        protected override void Start()
        {
            base.Start();

            ResourceType[] resourceTypes = { ResourceType.Wine };
            var resourceQuantities = new int[resourceTypes.Length];
            InventoryWhenCompleted = InitInventory(resourceTypes);
            OutputTypesWhenCompleted = new[] { ResourceType.Wine };

        }

        public override void StartResourceCreationCoroutine()
        {
            StartCoroutine(CreateResource( 15, ResourceType.Wine));
        }
    }
}