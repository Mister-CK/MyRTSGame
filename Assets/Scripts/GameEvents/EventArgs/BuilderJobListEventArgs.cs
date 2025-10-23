using Interface;
using System;
using System.Collections.Generic;
using MyRTSGame.Model;

public class BuilderJobListEventArgs : EventArgs, IGameEventArgs
{
    public List<BuilderJob> BuilderJobs { get; }

    public BuilderJobListEventArgs(List<BuilderJob> builderJobs)
    {
        BuilderJobs = builderJobs;
    }
}