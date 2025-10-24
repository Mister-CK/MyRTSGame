using Enums;

namespace Units.Model.Data
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