using System;
using MyRTSGame.Model;

public class BuildingResourceTypeEventArgs : EventArgs, IGameEventArgs
{
    public Building Building { get; }
    public ResourceType ResourceType { get; }

    public BuildingResourceTypeEventArgs(Building building, ResourceType resourceType)
    {
        Building = building;
        ResourceType = resourceType;
    }
}