using Interface;
using System;
using MyRTSGame.Model;
using MyRTSGame.Model.ResourceSystem.Model;

public class NaturalResourceEventArgs : EventArgs, IGameEventArgs
{
    public NaturalResource NaturalResource { get; }

    public NaturalResourceEventArgs(NaturalResource naturalResource)
    {
        NaturalResource = naturalResource;
    }
}