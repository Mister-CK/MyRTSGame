using System;
using MyRTSGame.Model;

public class BuilderEventArgs : EventArgs, IGameEventArgs
{
    public Builder Builder { get; }


    public BuilderEventArgs(Builder builder)
    {
        Builder = builder;
    }
}