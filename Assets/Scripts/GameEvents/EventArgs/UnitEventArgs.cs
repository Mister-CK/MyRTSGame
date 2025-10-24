using Interface;
using System;
using Units.Model.Component;

public class UnitEventArgs : EventArgs, IGameEventArgs
{
    public UnitComponent Unit { get; }

    public UnitEventArgs(UnitComponent unit)
    {
        Unit = unit;
    }
}