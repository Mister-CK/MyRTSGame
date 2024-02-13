namespace MyRTSGame.Model
{
    public class WeaponsSmith : Building
    {
        //Constructor
        public WeaponsSmith()
        {
            BuildingType = BuildingType.WeaponsSmith;
        }

        protected override void Start()
        {            
            base.Start();
            
            ResourceType[] resourceTypes = {ResourceType.Coal, ResourceType.Iron, ResourceType.Sword, ResourceType.Pike, ResourceType.CrossBow };
            var resourceQuantities = new int[resourceTypes.Length];
            InventoryWhenCompleted = InitInventory(resourceTypes, resourceQuantities);
            InputTypesWhenCompleted = new[] {ResourceType.Coal, ResourceType.Iron, };
            HasInput = true;
        }
    }
}