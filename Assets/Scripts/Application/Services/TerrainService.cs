using Buildings.Model;
using Enums;
using Interface;
using UnityEngine;

namespace Application.Services
{
    public class TerrainService: MonoBehaviour
    {
        [SerializeField] private GameEvent onNewJobNeeded;
        [SerializeField] private GameEvent onPlantResourceEvent;
        
        
        public void CreateJobNeededEvent(JobType jobType, IDestination destination, Building origin, ResourceType? resourceType, UnitType? unitType)
        {
            onNewJobNeeded.Raise(new CreateNewJobEventArgs(jobType, destination, origin, resourceType, unitType));
        }
    }
}