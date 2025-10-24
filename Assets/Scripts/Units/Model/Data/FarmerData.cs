using Enums;

namespace Units.Model.Data
{
    public class FarmerData : ResourceCollectorData
    {
        public FarmerData()
        {
            SetUnitType(UnitType.Farmer);
            SetResourceTypeToCollect(ResourceType.Stone);
        }
    }
}