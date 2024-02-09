using System;
using MyRTSGame.Model;

public class BuildingEventArgs : EventArgs, IGameEventArgs
{
    public Building Building { get; }

    public BuildingEventArgs(Building building)
    {
        Building = building;
    }
}