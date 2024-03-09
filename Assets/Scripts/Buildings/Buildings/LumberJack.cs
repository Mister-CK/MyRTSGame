namespace MyRTSGame.Model
{
    public class Lumberjack : ResourceBuilding
    {
        //Constructor
        public Lumberjack()
        {
            BuildingType = BuildingType.LumberJack;
        }

        protected override void Start()
        {
            base.Start();
            

            var resourceTypes = new[] { ResourceType.Lumber };
            var resourceQuantities = new int[resourceTypes.Length];
            InventoryWhenCompleted = InitInventory(resourceTypes, resourceQuantities);
        }
        
        public override void StartResourceCreationCoroutine()
        {
            StartCoroutine(buildingController.CreateResource(this, 5, ResourceType.Lumber));
        }
    }
}