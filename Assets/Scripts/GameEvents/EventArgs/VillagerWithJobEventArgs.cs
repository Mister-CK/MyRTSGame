using System;
using MyRTSGame.Model;

public class VillagerWithJobEventArgs : EventArgs, IGameEventArgs
{
    public Villager Villager { get; }
    public VillagerJob VillagerJob { get; }


    public VillagerWithJobEventArgs(Villager villager, VillagerJob villagerJob)
    {
        Villager = villager;
        VillagerJob = villagerJob;
    }
}