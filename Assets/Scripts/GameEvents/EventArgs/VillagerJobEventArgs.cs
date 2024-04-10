using System;
using MyRTSGame.Model;

public class VillagerJobEventArgs : EventArgs, IGameEventArgs
{
    public VillagerJob VillagerJob { get; }

    public VillagerJobEventArgs(VillagerJob villager)
    {
        VillagerJob = villager;
    }
}