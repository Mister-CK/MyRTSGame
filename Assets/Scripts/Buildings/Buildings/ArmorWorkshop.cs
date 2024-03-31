namespace MyRTSGame.Model
{
    public class ArmorWorkshop : WorkshopBuilding
    {
        //Constructor
        public ArmorWorkshop()
        {
            BuildingType = BuildingType.ArmorWorkshop;
        }

        protected override void Start()
        {            
            base.Start();
            
            ResourceType[] resourceTypes = {ResourceType.Wood, ResourceType.Leather, ResourceType.WoodenShield, ResourceType.LeatherArmor };
            var resourceQuantities = new int[resourceTypes.Length];
            InventoryWhenCompleted = InitInventory(resourceTypes, resourceQuantities);
            InputTypesWhenCompleted = new[] { ResourceType.Wood, ResourceType.Leather };
            HasInput = true;
        }
        
        public override void StartResourceCreationCoroutine()
        {
            StartCoroutine(CreateResourceFromQueue(15));
        }
    }
}