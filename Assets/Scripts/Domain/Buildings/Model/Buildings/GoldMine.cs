using Buildings.Model.BuildingGroups;
using Enums;

namespace Domain.Model
{
    public class GoldMine : ResourceBuilding
    {
        //Constructor
        public GoldMine()
        {
            BuildingType = BuildingType.GoldMine;
        }

        protected override void Start()
        {
            base.Start();

            OutputTypesWhenCompleted = new[] { ResourceType.GoldOre };

        }

        protected override void StartResourceCreationCoroutine()
        {
            StartCoroutine(CreateResource(15, ResourceType.GoldOre));
        }
    }
}