using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyRTSGame.Model
{
    public class BuildingController : MonoBehaviour
    {

        [SerializeField] private GameEvent onResourceRemovedFromBuilding;
        [SerializeField] private GameEvent onResourceAddedToBuilding;
        [SerializeField] private GameEvent onNewVillagerJobNeeded;
        [SerializeField] private GameEvent onNewBuilderJobNeeded;
        [SerializeField] private GameEvent onAddProductionJobEvent;
        [SerializeField] private GameEvent onRemoveProductionJobEvent;
        [SerializeField] private GameEvent onAddTrainingJobEvent;
        [SerializeField] private GameEvent onRemoveTrainingJobEvent;

        public static BuildingController Instance { get; private set; }

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
            onResourceAddedToBuilding.RegisterListener(OnResourceAdded);
            onResourceRemovedFromBuilding.RegisterListener(OnResourceRemoved);
            onAddProductionJobEvent.RegisterListener(OnAddProductionJob);
            onRemoveProductionJobEvent.RegisterListener(OnRemoveProductionJob);
            onAddTrainingJobEvent.RegisterListener(OnAddTrainingJob);
            onRemoveTrainingJobEvent.RegisterListener(OnRemoveTrainingJob);
        }

        private void OnDisable()
        {
            onResourceAddedToBuilding.UnregisterListener(OnResourceAdded);
            onResourceRemovedFromBuilding.UnregisterListener(OnResourceRemoved);
            onAddProductionJobEvent.RegisterListener(OnAddProductionJob);
            onRemoveProductionJobEvent.RegisterListener(OnRemoveProductionJob);
            
            onAddTrainingJobEvent.RegisterListener(OnAddTrainingJob);
            onRemoveProductionJobEvent.RegisterListener(OnRemoveProductionJob);
            
        }
        
        private static void OnResourceAdded(IGameEventArgs args)
        {
            if (args is BuildingResourceTypeQuantityEventArgs eventArgs)
            {
                eventArgs.Building.AddResource(eventArgs.ResourceType, eventArgs.Quantity);
            }
        }
        
        private static void OnResourceRemoved(IGameEventArgs args)
        {
            if (args is BuildingResourceTypeQuantityEventArgs eventArgs)
            {
                eventArgs.Building.RemoveResource(eventArgs.ResourceType, eventArgs.Quantity);
            }
        }
        
        public void CreateVillagerJobNeededEvent(Building building, ResourceType resourceType)
        {
            onNewVillagerJobNeeded.Raise(new BuildingResourceTypeEventArgs(building, resourceType));
        }
        
        public void CreateNewBuilderJobNeededEvent(Building building)
        {
            onNewBuilderJobNeeded.Raise(new BuildingEventArgs(building));
        }
        
        private void OnAddProductionJob(IGameEventArgs args)
        {
            if (args is WorkshopBuildingBuildingResourceTypeEventArgs eventArgs)
            {
                eventArgs.WorkshopBuilding.AddProductionJob(eventArgs.ResourceType);
            }
        }
        
        private void OnRemoveProductionJob(IGameEventArgs args)
        {
            if (args is WorkshopBuildingBuildingResourceTypeEventArgs eventArgs)
            {
                eventArgs.WorkshopBuilding.RemoveProductionJob(eventArgs.ResourceType);
            }
        }
        
        private void OnAddTrainingJob(IGameEventArgs args)
        {
            if (args is TrainingBuildingBuildingResourceTypeEventArgs eventArgs)
            {
                eventArgs.TrainingBuilding.AddTrainingJob(eventArgs.UnitType);
            }
        }
        
        private void OnRemoveTrainingJob(IGameEventArgs args)
        {
            if (args is TrainingBuildingBuildingResourceTypeEventArgs eventArgs)
            {
                eventArgs.TrainingBuilding.RemoveTrainingJob(eventArgs.UnitType);
            }
        }
    }
}