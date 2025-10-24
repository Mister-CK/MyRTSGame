using Interface;
using System;
using MyRTSGame.Model;
using Units.Model.Component;

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