using Buildings.Model;
using Enums;
using System.Collections.Generic;

namespace Domain.Model
{
    public class VillagerJob: Job
    {
        public Building Origin { get; set; }
        public ResourceType ResourceType { get; set; }
    }
}