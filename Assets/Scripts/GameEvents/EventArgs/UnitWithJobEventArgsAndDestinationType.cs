using Enums;
using Interface;
using System;
using JetBrains.Annotations;
using MyRTSGame.Model;
using Units.Model.Component;

public class UnitWithJobEventArgsAndDestinationType : EventArgs, IGameEventArgs
{
    public UnitComponent Unit { get; }
    public Job Job { get; }
    [CanBeNull]public DestinationType? DestinationType { get; }


    public UnitWithJobEventArgsAndDestinationType(UnitComponent unit, Job job, DestinationType? destinationType)
    {
        Unit = unit;
        Job = job;
        DestinationType = destinationType;
    }
}