using Enums;
using Interface;
using System;
using Units.Model.Component;

public class UnitWithJobTypeEventArgs : EventArgs, IGameEventArgs
{
    public UnitComponent Unit { get; }
    public JobType JobType { get; }

    public UnitWithJobTypeEventArgs(UnitComponent unit, JobType jobType)
    {
        Unit = unit;
        JobType = jobType;
    }
}