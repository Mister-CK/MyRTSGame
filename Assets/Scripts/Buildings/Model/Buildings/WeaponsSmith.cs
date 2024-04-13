using System.Collections.Generic;

namespace MyRTSGame.Model
{
    public class WeaponsSmith : WorkshopBuilding
    {
        ProductionJob SwordJob = new ProductionJob
        {
            Input = new Resource[]
            {
                new Resource { ResourceType = ResourceType.Coal, Quantity = 1 },
                new Resource { ResourceType = ResourceType.Iron, Quantity = 1 }
            },
            Output = new Resource { ResourceType = ResourceType.Sword, Quantity = 1 },
            Quantity = 0
        };
        ProductionJob PikeJob = new ProductionJob
        {
            Input = new Resource[]
            {
                new Resource { ResourceType = ResourceType.Coal, Quantity = 1 },
                new Resource { ResourceType = ResourceType.Iron, Quantity = 1 }
            },
            Output = new Resource { ResourceType = ResourceType.Pike, Quantity = 1 },
            Quantity = 0
        };
        ProductionJob CrossbowJob = new ProductionJob
        {
            Input = new Resource[]
            {
                new Resource { ResourceType = ResourceType.Coal, Quantity = 1 },
                new Resource { ResourceType = ResourceType.Iron, Quantity = 1 }
            },
            Output = new Resource { ResourceType = ResourceType.CrossBow, Quantity = 1 },
            Quantity = 0
        };
        
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
            InventoryWhenCompleted = InitInventory(resourceTypes);
            InputTypesWhenCompleted = new[] {ResourceType.Coal, ResourceType.Iron, };
            OutputTypesWhenCompleted = new[] {ResourceType.Sword, ResourceType.Pike, ResourceType.CrossBow };
            ProductionJobs = new List<ProductionJob>() {SwordJob, PikeJob, CrossbowJob};
            HasInput = true;
        }
        
        public override void StartResourceCreationCoroutine()
        {
            StartCoroutine(CreateResourceFromQueue(15));
        }
    }
}