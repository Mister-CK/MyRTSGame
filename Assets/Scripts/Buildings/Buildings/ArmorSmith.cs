namespace MyRTSGame.Model
{
    public class ArmorSmith : Building
    {
        //Constructor
        public ArmorSmith()
        {
            BuildingType = BuildingType.ArmorSmith;
        }

        protected override void Start()
        {            
            base.Start();
            
            ResourceType[] resourceTypes = {ResourceType.Coal, ResourceType.Iron, ResourceType.IronShield, ResourceType.IronArmor };
            var resourceQuantities = new int[resourceTypes.Length];
            InventoryWhenCompleted = InitInventory(resourceTypes, resourceQuantities);
            InputTypesWhenCompleted = new[] { ResourceType.Coal, ResourceType.Iron };
            HasInput = true;
        }
    }
}