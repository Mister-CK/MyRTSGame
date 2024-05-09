using System;
using JetBrains.Annotations;
using MyRTSGame.Model;

public class CreateNewJobEventArgs : EventArgs, IGameEventArgs
{
    public JobType JobType { get; }
    [CanBeNull] public Building Origin { get; }
    [CanBeNull] public IDestination Destination { get; }
    public ResourceType? ResourceType { get; }
    public UnitType? UnitType { get; }

    public CreateNewJobEventArgs(JobType jobType, [CanBeNull] IDestination destination, [CanBeNull] Building origin, ResourceType? resourceType, UnitType? unitType)
    {
        Origin = origin;
        Destination = destination;
        ResourceType = resourceType;
        JobType = jobType;
        UnitType = unitType;
    }
}