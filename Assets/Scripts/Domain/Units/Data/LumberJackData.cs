using Enums;

namespace Domain.Units.Data
{
    public class LumberJackData: ResourceCollectorData
    {
        public LumberJackData()
        {
            SetUnitType(UnitType.LumberJack);
            SetResourceTypeToCollect(ResourceType.Stone);
        }
    }
}