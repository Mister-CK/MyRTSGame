using Interface;
using System;
using Domain;
using Domain.Model.ResourceSystem.Model;

public class NaturalResourceEventArgs : EventArgs, IGameEventArgs
{
    public NaturalResource NaturalResource { get; }

    public NaturalResourceEventArgs(NaturalResource naturalResource)
    {
        NaturalResource = naturalResource;
    }
}