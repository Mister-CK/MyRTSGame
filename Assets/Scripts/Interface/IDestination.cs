
using UnityEngine;

namespace MyRTSGame.Model
{
    public interface IDestination
    {
        public void AddVillagerJobToThisBuilding(Job job);

        public void AddBuilderJobFromThisBuilding(Job job);

        public void AddConsumptionJobForThisBuilding(Job job);
        public Vector3 GetPosition();

        public BuildingType GetBuildingType();
        public void SetState(IBuildingState newState);

    }
}