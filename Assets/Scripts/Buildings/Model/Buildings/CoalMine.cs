namespace MyRTSGame.Model
{
    public class CoalMine : ResourceBuilding
    {
        //Constructor
        public CoalMine()
        {
            BuildingType = BuildingType.CoalMine;
        }

        protected override void Start()
        {
            base.Start();

            ResourceType[] resourceTypes = { ResourceType.Coal };
            var resourceQuantities = new int[resourceTypes.Length];
            InventoryWhenCompleted = InitInventory(resourceTypes);
            OutputTypesWhenCompleted = new[] { ResourceType.Coal };
        }

        public override void StartResourceCreationCoroutine()
        {
            StartCoroutine(CreateResource(15, ResourceType.Coal));
        }
    }
}