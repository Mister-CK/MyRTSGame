﻿using System.Collections.Generic;

namespace MyRTSGame.Model
{
    public class VillagerJob: Job
    {
        public Building Origin { get; set; }
        public ResourceType ResourceType { get; set; }
        public Villager Villager;
    }
}