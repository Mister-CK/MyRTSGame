﻿namespace MyRTSGame.Model
{
    public class Job
    {
        public Building Origin { get; set; }
        public Building Destination { get; set; }
        public ResourceType ResourceType { get; set; }
    }
}