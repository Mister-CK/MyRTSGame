
using UnityEngine;

namespace MyRTSGame.Model
{
    public interface IDestination
    {
        public void AddJobToDestination(Job job);
        public Vector3 GetPosition();

        public BuildingType GetBuildingType();
        public void SetState(IBuildingState newState);
    }
}