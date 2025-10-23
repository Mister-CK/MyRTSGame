using Enums;
using Interface;
using System;
using JetBrains.Annotations;
using MyRTSGame.Model;

public class UnitWithJobEventArgsAndDestinationType : EventArgs, IGameEventArgs
{
    public Unit Unit { get; }
    public Job Job { get; }
    [CanBeNull]public DestinationType? DestinationType { get; }


    public UnitWithJobEventArgsAndDestinationType(Unit unit, Job job, DestinationType? destinationType)
    {
        Unit = unit;
        Job = job;
        DestinationType = destinationType;
    }
}