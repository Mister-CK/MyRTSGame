using Buildings.Model;
using Interface;
using System;

public class BuildingEventArgs : EventArgs, IGameEventArgs
{
    public Building Building { get; }

    public BuildingEventArgs(Building building)
    {
        Building = building;
    }
}