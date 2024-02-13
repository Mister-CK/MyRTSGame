namespace MyRTSGame.Model
{
    public class School : Building
    {
        //Constructor
        public School()
        {
            BuildingType = BuildingType.School;
        }

        protected override void Start()
        {            
            base.Start();
            
            ResourceType[] resourceTypes = { ResourceType.Gold};
            var resourceQuantities = new int[resourceTypes.Length];
            InventoryWhenCompleted = InitInventory(resourceTypes, resourceQuantities);
            InputTypesWhenCompleted = new[] { ResourceType.Gold };
            HasInput = true;
        }
    }
}