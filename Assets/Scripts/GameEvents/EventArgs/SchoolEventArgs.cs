using System;
using MyRTSGame.Model;

public class TrainingBuildingEventArgs : EventArgs, IGameEventArgs
{
    public TrainingBuilding TrainingBuilding { get; }

    public TrainingBuildingEventArgs(TrainingBuilding trainingBuilding)
    {
        TrainingBuilding = trainingBuilding;
    }
}