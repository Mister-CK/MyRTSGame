using Buildings.Model.BuildingGroups;
using Enums;

namespace MyRTSGame.Model
{
    public class WheatFarm: ResourceBuilding
    {
        public WheatFarm()
        {
            BuildingType = BuildingType.WheatFarm;
        }

        protected override void Start()
        {
            base.Start();

            OutputTypesWhenCompleted = new[] { ResourceType.Wheat };
            OccupantType = UnitType.Farmer;
        }

        protected override void StartResourceCreationCoroutine()
        {
            //StartCoroutine(CreateResource(15, ResourceType.Wheat));
        }
    }
}