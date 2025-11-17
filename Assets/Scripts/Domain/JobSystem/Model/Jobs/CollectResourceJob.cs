using Enums;
using UnityEngine;

namespace Domain.Model
{
    public class CollectResourceJob: Job
    {
        public ResourceType ResourceType { get; set; }
    }
}