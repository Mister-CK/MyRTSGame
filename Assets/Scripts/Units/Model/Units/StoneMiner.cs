using Enums;

namespace MyRTSGame.Model
{
    public class StoneMiner: ResourceCollector
    {
        public StoneMiner()
        {
            UnitType = UnitType.StoneMiner;
            ResourceTypeToCollect  = ResourceType.Stone;
        }

        protected override void Start()
        {
            base.Start();
            IsLookingForBuilding = true;
        }
    }
}