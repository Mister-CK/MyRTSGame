using System;
using MyRTSGame.Model;

public class VillagerWithJobEventArgsAndDestinationtype : EventArgs, IGameEventArgs
{
    public Villager Villager { get; }
    public VillagerJob VillagerJob { get; }
    public DestinationType DestinationType { get; }


    public VillagerWithJobEventArgsAndDestinationtype(Villager villager, VillagerJob villagerJob, DestinationType destinationType)
    {
        Villager = villager;
        VillagerJob = villagerJob;
        DestinationType = destinationType;
    }
}