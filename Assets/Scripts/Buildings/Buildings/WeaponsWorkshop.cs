using System.Collections.Generic;

namespace MyRTSGame.Model
{
    public class WeaponsWorkshop : WorkshopBuilding
    {
        ProductionJob AxeJob = new ProductionJob
        {
            Input = new Resource[]
            {
                new Resource { ResourceType = ResourceType.Wood, Quantity = 2 },
            },
            Output = new Resource { ResourceType = ResourceType.Axe, Quantity = 1 },
            Quantity = 0
        };
        
        ProductionJob SpearJob = new ProductionJob
        {
            Input = new Resource[]
            {
                new Resource { ResourceType = ResourceType.Wood, Quantity = 2 },
            },
            Output = new Resource { ResourceType = ResourceType.Spear, Quantity = 1 },
            Quantity = 0
        };
        
        ProductionJob BowJob = new ProductionJob
        {
            Input = new Resource[]
            {
                new Resource { ResourceType = ResourceType.Wood, Quantity = 2 },
            },
            Output = new Resource { ResourceType = ResourceType.Bow, Quantity = 1 },
            Quantity = 0
        };
        
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
            ProductionJobs = new List<ProductionJob>() { AxeJob, SpearJob, BowJob };
            HasInput = true;
        }
        
        public override void StartResourceCreationCoroutine()
        {
            StartCoroutine(CreateResourceFromQueue(15));
        }
    }
}