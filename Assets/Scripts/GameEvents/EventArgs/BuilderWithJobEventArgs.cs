using System;
using MyRTSGame.Model;

public class BuilderWithJobEventArgs : EventArgs, IGameEventArgs
{
    public Builder Builder { get; }
    public BuilderJob BuilderJob { get; }


    public BuilderWithJobEventArgs(Builder builder, BuilderJob builderJob)
    {
        Builder = builder;
        BuilderJob = builderJob;
    }
}