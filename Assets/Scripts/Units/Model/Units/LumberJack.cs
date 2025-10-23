
using Enums;

namespace MyRTSGame.Model
{
    public class LumberJack: ResourceCollector
    {
        public LumberJack()
        {
            UnitType = UnitType.LumberJack;
            ResourceTypeToCollect  = ResourceType.Lumber;
        }

        protected override void Start()
        {
            base.Start();
            IsLookingForBuilding = true;
        }
    }
}