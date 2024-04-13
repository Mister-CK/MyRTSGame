namespace MyRTSGame.Model
{
    public class GoldMine : ResourceBuilding
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
            InventoryWhenCompleted = InitInventory(resourceTypes);
            OutputTypesWhenCompleted = new[] { ResourceType.GoldOre };

        }

        public override void StartResourceCreationCoroutine()
        {
            StartCoroutine(CreateResource(15, ResourceType.GoldOre));
        }
    }
}