using System;
using JetBrains.Annotations;
using MyRTSGame.Model;

public class CreateNewJobEventArgs : EventArgs, IGameEventArgs
{
    public JobType JobType { get; }
    [CanBeNull] public Building Origin { get; }
    [CanBeNull] public Building Destination { get; }
    public ResourceType? ResourceType { get; }

    public CreateNewJobEventArgs(JobType jobType, [CanBeNull] Building destination, [CanBeNull] Building origin, ResourceType? resourceType)
    {
        Origin = origin;
        Destination = destination;
        ResourceType = resourceType;
        JobType = jobType;
    }
}