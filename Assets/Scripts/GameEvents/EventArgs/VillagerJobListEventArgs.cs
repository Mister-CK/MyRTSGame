using System;
using System.Collections.Generic;
using MyRTSGame.Model;

public class VillagerJobListEventArgs : EventArgs, IGameEventArgs
{
    public List<VillagerJob> VillagerJobs { get; }
    public DestinationType DestinationType { get; }

    public VillagerJobListEventArgs(List<VillagerJob> villagerJobs, DestinationType destinationType)
    {
        VillagerJobs = villagerJobs;
        DestinationType = destinationType;

    }
}