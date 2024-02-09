using System;
using MyRTSGame.Model;

public class BuildingResourceEventArgs : EventArgs, IGameEventArgs
{
    public Building Building { get; }
    public Resource Resource { get; }

    public BuildingResourceEventArgs(Building building, Resource resource)
    {
        Building = building;
        Resource = resource;
    }
}