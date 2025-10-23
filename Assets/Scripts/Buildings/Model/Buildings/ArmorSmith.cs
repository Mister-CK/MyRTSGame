using Buildings.Model.BuildingGroups;
using Enums;
using System.Collections.Generic;

namespace MyRTSGame.Model
{
    public class ArmorSmith : WorkshopBuilding
    {
        ProductionJob IronShieldJob = new ProductionJob
        {
            Input = new Resource[]
            {
                new Resource { ResourceType = ResourceType.Coal, Quantity = 1 },
                new Resource { ResourceType = ResourceType.Iron, Quantity = 1 }
            },
            Output = new Resource { ResourceType = ResourceType.IronShield, Quantity = 1 },
            Quantity = 0
        };
        
        ProductionJob IronArmorJob = new ProductionJob
        {
            Input = new Resource[]
            {
                new Resource { ResourceType = ResourceType.Coal, Quantity = 1 },
                new Resource { ResourceType = ResourceType.Iron, Quantity = 1 }
            },
            Output = new Resource { ResourceType = ResourceType.IronArmor, Quantity = 1 },
            Quantity = 0
        };
        
        //Constructor
        public ArmorSmith()
        {
            BuildingType = BuildingType.ArmorSmith;
        }
        
        protected override void Start()
        {            
            base.Start();
            
            InputTypesWhenCompleted = new[] { ResourceType.Coal, ResourceType.Iron };
            OutputTypesWhenCompleted = new[] { ResourceType.IronShield, ResourceType.IronArmor };
            ProductionJobs = new List<ProductionJob>() {IronShieldJob, IronArmorJob};
            HasInput = true;
        }
        
        protected override void StartResourceCreationCoroutine()
        {
            StartCoroutine(CreateResourceFromQueue(15));
        }
    }
}