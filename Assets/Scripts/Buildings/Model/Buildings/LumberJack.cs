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
            InventoryWhenCompleted = InitInventory(resourceTypes);
            OutputTypesWhenCompleted = new[] { ResourceType.Lumber };
        }
        
        public override void StartResourceCreationCoroutine()
        {
            StartCoroutine(CreateResource(5, ResourceType.Lumber));
        }
    }
}