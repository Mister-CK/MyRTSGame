using Buildings.Model.BuildingGroups;
using Enums;
using Interface;
using System;
using MyRTSGame.Model;

public class WorkshopBuildingBuildingResourceTypeEventArgs : EventArgs, IGameEventArgs
{
    public WorkshopBuilding WorkshopBuilding { get; }
    public ResourceType ResourceType { get; }

    public WorkshopBuildingBuildingResourceTypeEventArgs(WorkshopBuilding building, ResourceType resourceType)
    {
        WorkshopBuilding = building;
        ResourceType = resourceType;
    }
}