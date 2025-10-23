using Buildings.Model.BuildingGroups;
using Enums;

namespace MyRTSGame.Model
{
    public class GoldSmelter : ProductionBuilding
    {
        //Constructor
        public GoldSmelter()
        {
            BuildingType = BuildingType.GoldSmelter;
        }

        protected override void Start()
        {            
            base.Start();
            
            InputTypesWhenCompleted = new[] { ResourceType.GoldOre, ResourceType.Coal};
            OutputTypesWhenCompleted = new[] { ResourceType.Gold};
            HasInput = true;
        }
        
        protected override void StartResourceCreationCoroutine()
        {
            Resource[] input = { new() { ResourceType = ResourceType.GoldOre, Quantity = 1 }, new() { ResourceType = ResourceType.Coal, Quantity = 1 } };
            Resource[] output = { new() { ResourceType = ResourceType.Gold, Quantity = 1 } };
            StartCoroutine(CreateOutputFromInput(10, input, output));
        }
    }
}