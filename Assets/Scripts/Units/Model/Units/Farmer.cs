using Enums;

namespace MyRTSGame.Model
{
    public class Farmer : ResourceCollector
    {
        public Farmer()
        {
            UnitType = UnitType.Farmer;
            ResourceTypeToCollect  = ResourceType.Wheat;
        }

        protected override void Start()
        {
            base.Start();
            IsLookingForBuilding = true;
        }
    }
}