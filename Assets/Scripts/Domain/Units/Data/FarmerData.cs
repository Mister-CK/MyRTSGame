using Enums;

namespace Domain.Units.Data
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