using System;
using MyRTSGame.Model;

public class UnitWithJobEventArgs : EventArgs, IGameEventArgs
{
    public Unit Unit { get; }
    public ConsumptionJob ConsumptionJob { get; }


    public UnitWithJobEventArgs(Unit unit, ConsumptionJob consumptionJob)
    {
        Unit = unit;
        ConsumptionJob = consumptionJob;
    }
}