namespace MyRTSGame.Model
{
    public class Lumberjack : Building
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
            var resourceQuantities = new[] { 0 };
            Inventory = InitInventory(resourceTypes, resourceQuantities);
            InventoryWhenCompleted = InitInventory(resourceTypes, resourceQuantities);
        }
        
        public override void StartResourceCreationCoroutine()
        {
            StartCoroutine(BuildingController.CreateResource(5, ResourceType.Lumber));
        }
    }
}