using Buildings.Model.BuildingGroups;
using Enums;

namespace Domain.Model
{
    public class PigFarm : ProductionBuilding
    {
        //Constructor
        public PigFarm()
        {
            BuildingType = BuildingType.PigFarm;
        }

        protected override void Start()
        {            
            base.Start();
            
            InputTypesWhenCompleted = new[] { ResourceType.Wheat};
            OutputTypesWhenCompleted = new[] { ResourceType.Pork, ResourceType.Hides};

        }
        
        protected override void StartResourceCreationCoroutine()
        {
            Resource[] input = { new() { ResourceType = ResourceType.Wheat, Quantity = 1 } };
            Resource[] output = { new() { ResourceType = ResourceType.Pork, Quantity = 1 }, new() { ResourceType = ResourceType.Hides, Quantity = 1}};
            StartCoroutine(CreateOutputFromInput(10, input, output));
        }
    }
}