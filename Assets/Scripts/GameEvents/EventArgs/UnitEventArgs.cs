using Interface;
using System;
using MyRTSGame.Model;

public class UnitEventArgs : EventArgs, IGameEventArgs
{
    public Unit Unit { get; }

    public UnitEventArgs(Unit unit)
    {
        Unit = unit;
    }
}