namespace MyRTSGame.Model
{
    public class ArmorSmith : WorkshopBuilding
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
            OutputTypesWhenCompleted = new[] { ResourceType.IronShield, ResourceType.IronArmor };
            HasInput = true;
        }
        
        public override void StartResourceCreationCoroutine()
        {
            StartCoroutine(CreateResourceFromQueue(15));
        }
    }
}