namespace MyRTSGame.Model
{
    public class GuardTower : ConsumptionBuilding
    {
        //Constructor
        public GuardTower()
        {
            BuildingType = BuildingType.GuardTower;
        }

        protected override void Start()
        {            
            base.Start();
            
            ResourceType[] resourceTypes = { ResourceType.Stone };
            var resourceQuantities = new int[resourceTypes.Length];
            InventoryWhenCompleted = InitInventory(resourceTypes);
            InputTypesWhenCompleted = new[] { ResourceType.Stone };
            HasInput = true;
        }
    }
}