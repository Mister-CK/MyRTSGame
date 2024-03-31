namespace MyRTSGame.Model
{
    public class WeaponsWorkshop : WorkshopBuilding
    {
        //Constructor
        public WeaponsWorkshop()
        {
            BuildingType = BuildingType.WeaponsWorkshop;
        }

        protected override void Start()
        {            
            base.Start();
            
            ResourceType[] resourceTypes = {ResourceType.Wood, ResourceType.Bow, ResourceType.Axe, ResourceType.Spear };
            var resourceQuantities = new int[resourceTypes.Length];
            InventoryWhenCompleted = InitInventory(resourceTypes, resourceQuantities);
            InputTypesWhenCompleted = new[] { ResourceType.Wood };
            OutputTypesWhenCompleted = new[] { ResourceType.Bow, ResourceType.Axe, ResourceType.Spear };
            HasInput = true;
        }
        
        public override void StartResourceCreationCoroutine()
        {
            StartCoroutine(CreateResourceFromQueue(15));
        }
    }
}