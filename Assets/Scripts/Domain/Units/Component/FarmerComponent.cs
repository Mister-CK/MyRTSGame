using Domain.Units.Data;

namespace Domain.Units.Component
{
    public class FarmerComponent : ResourceCollectorComponent
    {
        protected override void Start()
        {
            base.Start();
            Data.SetIsLookingForBuilding(true);
        }
        
        protected override UnitData CreateUnitData()
        {
            return new FarmerData();
        }
    }
}