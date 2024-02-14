using System;
using MyRTSGame.Model;

public class BuildingResourceTypeQuantityEventArgs : EventArgs, IGameEventArgs
{
    public Building Building { get; }
    public ResourceType ResourceType { get; }
    public int Quantity { get; }

    public BuildingResourceTypeQuantityEventArgs(Building building, ResourceType resourceType, int quantity)
    {
        Building = building;
        ResourceType = resourceType;
        Quantity = quantity;
    }
}
