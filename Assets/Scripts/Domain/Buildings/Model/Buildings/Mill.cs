using Buildings.Model.BuildingGroups;
using Enums;

namespace Domain.Model
{
    public class Mill : ProductionBuilding
    {
        //Constructor
        public Mill()
        {
            BuildingType = BuildingType.Mill;
        }

        protected override void Start()
        {
            base.Start();

            InputTypesWhenCompleted = new[] { ResourceType.Wheat };
            OutputTypesWhenCompleted = new[] { ResourceType.Flour};

        }

        protected override void StartResourceCreationCoroutine()
        {
            Resource[] input = { new() { ResourceType = ResourceType.Wheat, Quantity = 1 } };
            Resource[] output = { new() { ResourceType = ResourceType.Flour, Quantity = 1 } };
            StartCoroutine(CreateOutputFromInput(10, input, output));
        }
    }
}