using System.Collections.Generic;

namespace MyRTSGame.Model
{
    public class VillagerJob: Job
    {
        public Building Origin { get; set; }
        public Building Destination { get; set; }
        public ResourceType ResourceType { get; set; }
        
        public void DeleteVillagerJobs(DestinationType destinationType)
        {
            if (IsInProgress()) return;
            if (destinationType == DestinationType.Origin)
            {
                return;
            }
            return;
        }
    }
}