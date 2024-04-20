using System;
using MyRTSGame.Model;

public class UnitWithJobTypeEventArgs : EventArgs, IGameEventArgs
{
    public Unit Unit { get; }
    public JobType JobType { get; }

    public UnitWithJobTypeEventArgs(Unit unit, JobType jobType)
    {
        Unit = unit;
        JobType = jobType;
    }
}