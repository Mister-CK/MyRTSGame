namespace MyRTSGame.Model
{
    public class CoalMine : Building
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
            InventoryWhenCompleted = InitInventory(resourceTypes, resourceQuantities);
        }

        public override void StartResourceCreationCoroutine()
        {
            StartCoroutine(buildingController.CreateResource(this, 15, ResourceType.Coal));
        }
    }
}