using System;
using MyRTSGame.Model;

public class VillagerEventArgs : EventArgs, IGameEventArgs
{
    public Villager Villager { get; }


    public VillagerEventArgs(Villager villager)
    {
        Villager = villager;
    }
}