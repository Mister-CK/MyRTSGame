using System;
using MyRTSGame.Model;

public class TrainingBuildingBuildingResourceTypeEventArgs : EventArgs, IGameEventArgs
{
    public TrainingBuilding TrainingBuilding { get; }
    public UnitType UnitType { get; }

    public TrainingBuildingBuildingResourceTypeEventArgs(TrainingBuilding building, UnitType unitType)
    {
        TrainingBuilding = building;
        UnitType = unitType;
    }
}