using Enums;

namespace Interface
{
    public interface IResourceRow
    {
        public ResourceType ResourceType { get; set; }
        public void UpdateQuantity(int newQuantity);
        public void UpdateJobs(int newQuantity);
    }
}
