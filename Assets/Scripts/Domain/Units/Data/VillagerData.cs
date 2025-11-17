using Enums;

namespace Domain.Units.Data
{
    public class VillagerData : UnitData
    {
        private bool _hasResource;
        
        public VillagerData()
        {
            SetUnitType(UnitType.Villager);
        }
        
        public bool GetHasResource()
        {
            return _hasResource;
        }

        public void SetHasResource(bool hasResource)
        {
            _hasResource = hasResource;
        }
    }
}
