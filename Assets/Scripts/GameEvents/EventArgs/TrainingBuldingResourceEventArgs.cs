using Buildings.Model.BuildingGroups;
using Enums;
using Interface;
using System;
using MyRTSGame.Model;

public class TrainingBuildingBuildingResourceTypeEventArgs : EventArgs, IGameEventArgs
{
    public TrainingBuilding TrainingBuilding { get; }
    public UnitType UnitType { get; }

    public TrainingBuildingBuildingResourceTypeEventArgs(TrainingBuilding trainingBuilding, UnitType unitType)
    {
        TrainingBuilding = trainingBuilding;
        UnitType = unitType;
    }
}