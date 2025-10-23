using Interface;
using System;
using MyRTSGame.Model;

public class UnitWithJobEventArgs : EventArgs, IGameEventArgs
{
    public Unit Unit { get; }
    public Job Job { get; }
    
    public UnitWithJobEventArgs(Unit unit, Job job)
    {
        Unit = unit;
        Job = job;
    }
}