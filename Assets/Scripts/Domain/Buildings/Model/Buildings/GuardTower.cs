using Buildings.Model.BuildingGroups;
using Enums;

namespace Domain.Model
{
    public class GuardTower : ConsumptionBuilding
    {
        //Constructor
        public GuardTower()
        {
            BuildingType = BuildingType.GuardTower;
        }

        protected override void Start()
        {            
            base.Start();
            
            InputTypesWhenCompleted = new[] { ResourceType.Stone };

        }
    }
}