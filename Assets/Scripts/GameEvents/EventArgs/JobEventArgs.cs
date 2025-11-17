using Interface;
using System;
using Domain.Model;

public class JobEventArgs : EventArgs, IGameEventArgs
{
    public Job Job { get; }
    
    public JobEventArgs(Job job)
    {
        Job = job;
    }
}