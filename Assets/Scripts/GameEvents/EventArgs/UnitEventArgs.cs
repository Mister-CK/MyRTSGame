using Interface;
using System;
using Domain.Units.Component;

public class UnitEventArgs : EventArgs, IGameEventArgs
{
    public UnitComponent Unit { get; }

    public UnitEventArgs(UnitComponent unit)
    {
        Unit = unit;
    }
}