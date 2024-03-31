namespace MyRTSGame.Model
{
    public class SiegeWorkshop : SpecialBuilding
    {
        //Constructor
        public SiegeWorkshop()
        {
            BuildingType = BuildingType.SiegeWorkshop;
        }

        protected override void Start()
        {            
            base.Start();
            
            ResourceType[] resourceTypes = {ResourceType.Wood, ResourceType.Iron};
            var resourceQuantities = new int[resourceTypes.Length];
            InventoryWhenCompleted = InitInventory(resourceTypes, resourceQuantities);
            InputTypesWhenCompleted = new[] {ResourceType.Wood, ResourceType.Iron, };
            HasInput = true;
        }
    }
}