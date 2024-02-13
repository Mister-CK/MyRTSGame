namespace MyRTSGame.Model
{
    public class Restaurant : Building
    {
        //Constructor
        public Restaurant()
        {
            BuildingType = BuildingType.Restaurant;
        }

        protected override void Start()
        {            
            base.Start();
            
            ResourceType[] resourceTypes = { ResourceType.Bread, ResourceType.Fish, ResourceType.Sausage, ResourceType.Wine};
            var resourceQuantities = new int[resourceTypes.Length];
            InventoryWhenCompleted = InitInventory(resourceTypes, resourceQuantities);
            InputTypesWhenCompleted = new[] { ResourceType.Bread, ResourceType.Fish, ResourceType.Sausage, ResourceType.Wine};
            HasInput = true;
        }
    }
}