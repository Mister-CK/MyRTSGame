using System;
using MyRTSGame.Model;

public class BuilderJobEventArgs : EventArgs, IGameEventArgs
{
    public BuilderJob BuilderJob { get; }


    public BuilderJobEventArgs(BuilderJob builderJob)
    {
        BuilderJob = builderJob;
    }
}