using UnityEngine;

namespace MyRTSGame.Model
{
    public class CollectResourceJob: Job
    {
        public ResourceType ResourceType { get; set; }
        public Transform TargetTransform { get; set; }

    }
}