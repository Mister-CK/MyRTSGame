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
            InventoryWhenCompleted = InitInventory(resourceTypes, resourceQuantities);
        }

        public override void StartResourceCreationCoroutine()
        {
            StartCoroutine(buildingController.CreateResource(this, 15, ResourceType.IronOre));
        }
    }
}