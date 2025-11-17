using Buildings.Model.BuildingGroups;
using Enums;

namespace Domain.Model
{
    public class StoneQuarry : ResourceBuilding
    {
        //Constructor
        public StoneQuarry()
        {
            BuildingType = BuildingType.StoneQuarry;
        }

        protected override void Start()
        {
            base.Start();

            OutputTypesWhenCompleted = new[] { ResourceType.Stone };
            OccupantType = UnitType.StoneMiner;
        }

        protected override void StartResourceCreationCoroutine()
        {
            // StartCoroutine(CreateResource( 15, ResourceType.Stone));
        }
    }
}