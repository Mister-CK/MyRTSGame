using Buildings.Model;
using Enums;

namespace Units.Model.Data
{
    public class ResourceCollectorData : UnitData
    {
        private Building _building;
        private bool _hasResource = false;
        
        public ResourceType ResourceTypeToCollect { get; private set; }

        public Building GetBuilding() => _building;
        public bool GetHasResource() => _hasResource;

        
        public void SetBuilding(Building building)
        {
            _building = building;
        }
        
        public void SetHasResource(bool hasResource)
        {
            _hasResource = hasResource;
        }

        public void SetResourceTypeToCollect(ResourceType resourceType)
        {
            ResourceTypeToCollect = resourceType;
        }
    }
}
