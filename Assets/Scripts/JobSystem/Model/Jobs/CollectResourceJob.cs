using UnityEngine;

namespace MyRTSGame.Model
{
    public class CollectResourceJob: Job
    {
        public ResourceType ResourceType { get; set; }
    }
}