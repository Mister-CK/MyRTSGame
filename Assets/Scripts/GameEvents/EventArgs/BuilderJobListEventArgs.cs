using Interface;
using System;
using System.Collections.Generic;
using Domain.Model;

public class BuilderJobListEventArgs : EventArgs, IGameEventArgs
{
    public List<BuilderJob> BuilderJobs { get; }

    public BuilderJobListEventArgs(List<BuilderJob> builderJobs)
    {
        BuilderJobs = builderJobs;
    }
}