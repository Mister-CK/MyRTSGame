using Units.Model.Data;

namespace Units.Model.Component
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