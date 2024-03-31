namespace MyRTSGame.Model
{
    public class WheatFarm: ResourceBuilding
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
            InventoryWhenCompleted = InitInventory(resourceTypes, resourceQuantities);
            OutputTypesWhenCompleted = new[] { ResourceType.Wheat };
        }

        public override void StartResourceCreationCoroutine()
        {
            StartCoroutine(CreateResource(15, ResourceType.Wheat));
        }
    }
}