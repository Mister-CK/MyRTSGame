using Buildings.Model;
using Enums;

namespace Domain.Units.Data
{
    public class ResourceCollectorData : UnitData
    {
        public Building Building { get; private set; }
        public bool HasResource { get; private set; }
        public ResourceType ResourceTypeToCollect { get; private set; }
        
        public void SetBuilding(Building building)
        {
            Building = building;
        }
        
        public void SetHasResource(bool hasResource)
        {
            HasResource = hasResource;
        }

        public void SetResourceTypeToCollect(ResourceType resourceType)
        {
            ResourceTypeToCollect = resourceType;
        }
    }
}
