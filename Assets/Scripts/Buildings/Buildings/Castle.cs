namespace MyRTSGame.Model
{
    public class Castle : Building
    {
        //Constructor
        public Castle()
        {
            BuildingType = BuildingType.Castle;
        }

        protected override void Start()
        {            
            base.Start();
            
            ResourceType[] resourceTypes =
            {
                ResourceType.Sword, 
                ResourceType.Pike, 
                ResourceType.CrossBow, 
                ResourceType.Axe, 
                ResourceType.Bow, 
                ResourceType.Spear, 
                ResourceType.IronArmor, 
                ResourceType.LeatherArmor, 
                ResourceType.Horses, 
                ResourceType.WoodenShield, 
                ResourceType.IronShield
            };
            var resourceQuantities = new int[resourceTypes.Length];
            InventoryWhenCompleted = InitInventory(resourceTypes, resourceQuantities);
            InputTypesWhenCompleted = new[]
            {
                ResourceType.Sword,
                ResourceType.Pike,
                ResourceType.CrossBow,
                ResourceType.Axe,
                ResourceType.Bow,
                ResourceType.Spear,
                ResourceType.IronArmor,
                ResourceType.LeatherArmor,
                ResourceType.Horses,
                ResourceType.WoodenShield,
                ResourceType.IronShield
            };
            HasInput = true;
        }
    }
}