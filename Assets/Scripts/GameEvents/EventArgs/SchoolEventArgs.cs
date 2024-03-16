using System;
using MyRTSGame.Model;

public class SchoolEventArgs : EventArgs, IGameEventArgs
{
    public School School { get; }

    public SchoolEventArgs(School school)
    {
        School = school;
    }
}