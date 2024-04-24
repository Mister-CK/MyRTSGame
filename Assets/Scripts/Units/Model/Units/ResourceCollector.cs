namespace MyRTSGame.Model
{
    public class ResourceCollector: Unit
    {
        protected ResourceType ResourceTypeToCollect;
        
        public ResourceType GetResourceToCollect()
        {
            return ResourceTypeToCollect;
        }

        public void SetResourceTypeToCollect(ResourceType resourceType)
        {
            ResourceTypeToCollect = resourceType;
        }
    }
}