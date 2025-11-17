using Enums;

namespace Domain.Units.Data
{
    public class StoneMinerData: ResourceCollectorData
    {
        public StoneMinerData()
        {
            SetUnitType(UnitType.StoneMiner);
            SetResourceTypeToCollect(ResourceType.Stone);
        }
    }
}