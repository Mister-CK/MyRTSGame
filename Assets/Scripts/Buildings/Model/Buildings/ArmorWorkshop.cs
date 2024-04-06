using System.Collections.Generic;

namespace MyRTSGame.Model
{
    public class ArmorWorkshop : WorkshopBuilding
    {
        ProductionJob WoodenShieldJob = new ProductionJob
        {
            Input = new Resource[]
            {
                new Resource { ResourceType = ResourceType.Wood, Quantity = 2 },
            },
            Output = new Resource { ResourceType = ResourceType.WoodenShield, Quantity = 1 },
            Quantity = 0
        };
        
        ProductionJob LeatherArmorJob = new ProductionJob
        {
            Input = new Resource[]
            {
                new Resource { ResourceType = ResourceType.Leather, Quantity = 2 },
            },
            Output = new Resource { ResourceType = ResourceType.LeatherArmor, Quantity = 1 },
            Quantity = 0
        };
        
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
            OutputTypesWhenCompleted = new[] { ResourceType.WoodenShield, ResourceType.LeatherArmor };
            ProductionJobs = new List<ProductionJob>() {WoodenShieldJob, LeatherArmorJob};
            HasInput = true;
        }
        
        public override void StartResourceCreationCoroutine()
        {
            StartCoroutine(CreateResourceFromQueue(15));
        }
    }
}