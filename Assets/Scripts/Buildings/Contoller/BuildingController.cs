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
        [SerializeField] private GameEvent onNewUnitEvent;
        [SerializeField] private GameEvent onUpdateUIViewForBuildingEvent;
        [SerializeField] private GameEvent onDeleteBuildingEvent; 
        [SerializeField] private GameEvent onNewVillagerJobCreated;
        [SerializeField] private GameEvent onDeleteVillagerJobsEvent;
        [SerializeField] private GameEvent onDeleteBuilderJobsEvent;
        [SerializeField] private GameEvent onNewBuilderJobCreated;
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
            onDeleteBuildingEvent.RegisterListener(HandleDeleteBuildingEvent);
            onNewVillagerJobCreated.RegisterListener(HandleNewVillagerJobCreated);
            onNewBuilderJobCreated.RegisterListener(HandleNewBuilderJobCreated);
        }

        private void OnDisable()
        {
            onResourceAddedToBuilding.UnregisterListener(OnResourceAdded);
            onResourceRemovedFromBuilding.UnregisterListener(OnResourceRemoved);
            onAddProductionJobEvent.UnregisterListener(OnAddProductionJob);
            onRemoveProductionJobEvent.UnregisterListener(OnRemoveProductionJob);
            onAddTrainingJobEvent.UnregisterListener(OnAddTrainingJob);
            onRemoveProductionJobEvent.UnregisterListener(OnRemoveProductionJob);
            onDeleteBuildingEvent.UnregisterListener(HandleDeleteBuildingEvent);
            onNewVillagerJobCreated.UnregisterListener(HandleNewVillagerJobCreated);
            onNewBuilderJobCreated.UnregisterListener(HandleNewBuilderJobCreated);
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
        
        public void CreateNewUnitEvent(TrainingBuilding trainingBuilding, UnitType unitType)
        {
            onNewUnitEvent.Raise(new TrainingBuildingUnitTypeEventArgs(trainingBuilding, unitType));
        }
        
        public void CreateUpdateViewForBuildingEvent(Building building)
        {
            onUpdateUIViewForBuildingEvent.Raise(new BuildingEventArgs(building));
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

        private void HandleDeleteBuildingEvent(IGameEventArgs args)
        {
            if (args is BuildingEventArgs eventArgs)
            {
                eventArgs.Building.DeleteBuilding();
            }
        }

        private void HandleNewVillagerJobCreated(IGameEventArgs args)
        {
            if (args is not VillagerJobEventArgs eventArgs) return;
            
            eventArgs.VillagerJob.Origin.AddVillagerJobFromThisBuilding(eventArgs.VillagerJob);
            eventArgs.VillagerJob.Destination.AddVillagerJobToThisBuilding(eventArgs.VillagerJob);
        }
        
        private void HandleNewBuilderJobCreated(IGameEventArgs args)
        {
            if (args is not BuilderJobEventArgs eventArgs) return;
            
            eventArgs.BuilderJob.Destination.AddBuilderJobFromThisBuilding(eventArgs.BuilderJob);
        }

        public void CreateDeleteJobsForBuildingEvent(List<VillagerJob> villagerJobsFromThisBuilding, List<VillagerJob> villagerJobsToThisBuilding, List <BuilderJob> builderJobsForThisBuilding)
        {
            onDeleteVillagerJobsEvent.Raise(new VillagerJobListEventArgs(villagerJobsFromThisBuilding, DestinationType.Origin));
            onDeleteVillagerJobsEvent.Raise(new VillagerJobListEventArgs(villagerJobsToThisBuilding, DestinationType.Destination));
            onDeleteBuilderJobsEvent.Raise(new BuilderJobListEventArgs(builderJobsForThisBuilding));
        }
    }
}