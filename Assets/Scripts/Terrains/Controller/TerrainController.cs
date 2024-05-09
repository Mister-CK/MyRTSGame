using System;
using UnityEngine;

namespace MyRTSGame.Model.Terrains
{
    public class TerrainController: MonoBehaviour
    {
        [SerializeField] private GameEvent onNewJobNeeded;
        
        public static TerrainController Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        private void OnEnable()
        {
            throw new NotImplementedException();
        }
        
        private void OnDisable()
        {
            throw new NotImplementedException();
        }
        
        public void CreateJobNeededEvent(JobType jobType, IDestination destination, Building origin, ResourceType? resourceType, UnitType? unitType)
        {
            onNewJobNeeded.Raise(new CreateNewJobEventArgs(jobType, destination, origin, resourceType, unitType));
        }
    }
}