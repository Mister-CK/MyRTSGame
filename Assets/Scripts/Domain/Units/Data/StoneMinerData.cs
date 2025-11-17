using Enums;

namespace Units.Model.Data
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