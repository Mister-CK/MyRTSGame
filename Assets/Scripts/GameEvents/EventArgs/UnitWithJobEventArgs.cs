using Interface;
using System;
using Domain.Model;
using Domain.Units.Component;

public class UnitWithJobEventArgs : EventArgs, IGameEventArgs
{
    public UnitComponent Unit { get; }
    public Job Job { get; }
    
    public UnitWithJobEventArgs(UnitComponent unit, Job job)
    {
        Unit = unit;
        Job = job;
    }
}