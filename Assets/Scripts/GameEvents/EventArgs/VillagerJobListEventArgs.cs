using Enums;
using Interface;
using System;
using System.Collections.Generic;
using Domain.Model;

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