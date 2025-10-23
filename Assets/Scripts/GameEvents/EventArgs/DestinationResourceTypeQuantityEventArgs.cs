using Enums;
using Interface;
using System;

public class DestinationResourceTypeQuantityEventArgs : EventArgs, IGameEventArgs
{
    public IDestination Destination { get; }
    public ResourceType ResourceType { get; }
    public int Quantity { get; }

    public DestinationResourceTypeQuantityEventArgs(IDestination destination, ResourceType resourceType, int quantity)
    {
        Destination = destination;
        ResourceType = resourceType;
        Quantity = quantity;
    }
}
