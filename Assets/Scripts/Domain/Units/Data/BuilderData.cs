using Enums;

namespace Domain.Units.Data
{
    public class BuilderData : UnitData
    {
        public BuilderData()
        {
            SetUnitType(UnitType.Builder);
        }
    }
}