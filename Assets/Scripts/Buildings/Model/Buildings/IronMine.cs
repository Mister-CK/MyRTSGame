namespace MyRTSGame.Model
{
    public class IronMine : ResourceBuilding
    {
        //Constructor
        public IronMine()
        {
            BuildingType = BuildingType.IronMine;
        }

        protected override void Start()
        {
            base.Start();

            ResourceType[] resourceTypes = { ResourceType.IronOre };
            var resourceQuantities = new int[resourceTypes.Length];
            InventoryWhenCompleted = InitInventory(resourceTypes);
            OutputTypesWhenCompleted = new[] { ResourceType.IronOre };

        }

        public override void StartResourceCreationCoroutine()
        {
            StartCoroutine(CreateResource( 15, ResourceType.IronOre));
        }
    }
}