using Buildings.Model;
using Enums;
using Interface;
using MyRTSGame.Model;
using UnityEngine;

namespace Application
{
    public class TerrainService: MonoBehaviour
    {
        [SerializeField] private GameEvent onNewJobNeeded;
        [SerializeField] private GameEvent onPlantResourceEvent;

        private void OnEnable()
        {
        }
        
        private void OnDisable()
        {
        }
        
        public void CreateJobNeededEvent(JobType jobType, IDestination destination, Building origin, ResourceType? resourceType, UnitType? unitType)
        {
            onNewJobNeeded.Raise(new CreateNewJobEventArgs(jobType, destination, origin, resourceType, unitType));
        }
        public void CreatePlantResourceEvent(Job job)
        {
            onPlantResourceEvent.Raise(new JobEventArgs(job));
        }
    }
}