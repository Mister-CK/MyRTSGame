using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyRTSGame.Model
{
    public class BuildingController : MonoBehaviour
    {

        [SerializeField] private GameEvent onResourceRemovedFromDestination;
        [SerializeField] private GameEvent onResourceAddedToBuilding;
        [SerializeField] private GameEvent onNewJobNeeded;
        [SerializeField] private GameEvent onAddProductionJobEvent;
        [SerializeField] private GameEvent onRemoveProductionJobEvent;
        [SerializeField] private GameEvent onAddTrainingJobEvent;
        [SerializeField] private GameEvent onRemoveTrainingJobEvent;
        [SerializeField] private GameEvent onNewUnitEvent;
        [SerializeField] private GameEvent onUpdateUIViewForBuildingEvent;
        [SerializeField] private GameEvent onDeleteBuildingEvent; 
        [SerializeField] private GameEvent onDeleteVillagerJobsEvent;
        [SerializeField] private GameEvent onDeleteBuilderJobsEvent;
        [SerializeField] private GameEvent onNewJobCreated;
        [SerializeField] private GameEvent onDeleteJobEvent;
        [SerializeField] private GameEvent onCompleteJobEvent;
        [SerializeField] private GameEvent onRemoveOccupantFromBuildingEvent;
        [SerializeField] private GameEvent onDeleteBuildingForOccupantEvent;
        
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
            onResourceRemovedFromDestination.RegisterListener(OnResourceRemoved);
            onAddProductionJobEvent.RegisterListener(OnAddProductionJob);
            onRemoveProductionJobEvent.RegisterListener(OnRemoveProductionJob);
            onAddTrainingJobEvent.RegisterListener(OnAddTrainingJob);
            onRemoveTrainingJobEvent.RegisterListener(OnRemoveTrainingJob);
            onDeleteBuildingEvent.RegisterListener(HandleDeleteBuildingEvent);
            onNewJobCreated.RegisterListener(HandleNewJobCreated);
            onDeleteJobEvent.RegisterListener(HandleDeleteJobEvent);
            onCompleteJobEvent.RegisterListener(HandleCompleteJobEvent);
            onRemoveOccupantFromBuildingEvent.RegisterListener(HandleOnRemoveOccupantFromBuildingEvent);
        }

        private void OnDisable()
        {
            onResourceAddedToBuilding.UnregisterListener(OnResourceAdded);
            onResourceRemovedFromDestination.UnregisterListener(OnResourceRemoved);
            onAddProductionJobEvent.UnregisterListener(OnAddProductionJob);
            onRemoveProductionJobEvent.UnregisterListener(OnRemoveProductionJob);
            onAddTrainingJobEvent.UnregisterListener(OnAddTrainingJob);
            onRemoveProductionJobEvent.UnregisterListener(OnRemoveProductionJob);
            onDeleteBuildingEvent.UnregisterListener(HandleDeleteBuildingEvent);
            onNewJobCreated.RegisterListener(HandleNewJobCreated);
            onDeleteJobEvent.UnregisterListener(HandleDeleteJobEvent);
            onCompleteJobEvent.UnregisterListener(HandleCompleteJobEvent);
            onRemoveOccupantFromBuildingEvent.UnregisterListener(HandleOnRemoveOccupantFromBuildingEvent);

        }
        
        private static void OnResourceAdded(IGameEventArgs args)
        {
            if (args is not DestinationResourceTypeQuantityEventArgs eventArgs) return;
            if (eventArgs.Destination is not Building building) return;
            building.AddResource(eventArgs.ResourceType, eventArgs.Quantity);
        }
        
        private static void OnResourceRemoved(IGameEventArgs args)
        {
            if (args is not DestinationResourceTypeQuantityEventArgs eventArgs) return;
            if (eventArgs.Destination is not Building building) return;
            building.RemoveResource(eventArgs.ResourceType, eventArgs.Quantity);
        }
        
        public void CreateJobNeededEvent(JobType jobType, Building destination, Building origin, ResourceType? resourceType, UnitType? unitType)
        {
            onNewJobNeeded.Raise(new CreateNewJobEventArgs(jobType, destination, origin, resourceType, unitType));
        }
        
        public void CreateNewUnitEvent(TrainingBuilding trainingBuilding, UnitType unitType)
        {
            onNewUnitEvent.Raise(new TrainingBuildingUnitTypeEventArgs(trainingBuilding, unitType));
        }
        
        public void CreateUpdateViewForBuildingEvent(Building building)
        {
            onUpdateUIViewForBuildingEvent.Raise(new BuildingEventArgs(building));
        }
        
        private static void OnAddProductionJob(IGameEventArgs args)
        {
            if (args is WorkshopBuildingBuildingResourceTypeEventArgs eventArgs)
            {
                eventArgs.WorkshopBuilding.AddProductionJob(eventArgs.ResourceType);
            }
        }
        
        private static void OnRemoveProductionJob(IGameEventArgs args)
        {
            if (args is WorkshopBuildingBuildingResourceTypeEventArgs eventArgs)
            {
                eventArgs.WorkshopBuilding.RemoveProductionJob(eventArgs.ResourceType);
            }
        }
        
        private static void OnAddTrainingJob(IGameEventArgs args)
        {
            if (args is TrainingBuildingBuildingResourceTypeEventArgs eventArgs)
            {
                eventArgs.TrainingBuilding.AddTrainingJob(eventArgs.UnitType);
            }
        }
        
        private static void OnRemoveTrainingJob(IGameEventArgs args)
        {
            if (args is TrainingBuildingBuildingResourceTypeEventArgs eventArgs)
            {
                eventArgs.TrainingBuilding.RemoveTrainingJob(eventArgs.UnitType);
            }
        }

        private static void HandleDeleteBuildingEvent(IGameEventArgs args)
        {
            if (args is BuildingEventArgs eventArgs)
            {
                eventArgs.Building.DeleteBuilding();
            }
        }

        private static void HandleNewJobCreated(IGameEventArgs args)
        {
            if (args is not JobEventArgs eventArgs) return;
            if (eventArgs.Job is CollectResourceJob) return;
            eventArgs.Job.Destination.AddJobToDestination(eventArgs.Job);
            if (eventArgs.Job is VillagerJob villagerJob)
            {
                villagerJob.Origin.AddVillagerJobFromThisBuilding(villagerJob);
            }
        }

        public void CreateDeleteJobsForBuildingEvent(List<VillagerJob> villagerJobsFromThisBuilding, List<VillagerJob> villagerJobsToThisBuilding, List <BuilderJob> builderJobsForThisBuilding)
        {
            onDeleteVillagerJobsEvent.Raise(new VillagerJobListEventArgs(villagerJobsFromThisBuilding, DestinationType.Origin));
            onDeleteVillagerJobsEvent.Raise(new VillagerJobListEventArgs(villagerJobsToThisBuilding, DestinationType.Destination));
            onDeleteBuilderJobsEvent.Raise(new BuilderJobListEventArgs(builderJobsForThisBuilding));
        }
        
        private void HandleDeleteJobEvent(IGameEventArgs args)
        {
            if (args is not JobEventArgs eventArgs) return;
            if (eventArgs.Job.Destination is not Building building) return;
            building.RemoveJobFromDestination(eventArgs.Job);
            if (eventArgs.Job is VillagerJob villagerJob)
            {
                villagerJob.Origin.RemoveVillagerJobFromThisBuilding(villagerJob);
            }
        }

        private void HandleCompleteJobEvent(IGameEventArgs args)
        {
            if (args is not JobEventArgs eventArgs) return;
            if (eventArgs.Job.Destination is not Building building) return;
            if (eventArgs.Job is LookingForBuildingJob lookingForBuildingJob)
            {
                building.SetOccupant(lookingForBuildingJob.Unit);
            }
            building.RemoveJobFromDestination(eventArgs.Job);
            if (eventArgs.Job is VillagerJob villagerJob)
            {
                villagerJob.Origin.RemoveVillagerJobFromThisBuilding(villagerJob);
            }
        }

        private void HandleOnRemoveOccupantFromBuildingEvent(IGameEventArgs args)
        {
            if (args is not BuildingEventArgs eventArgs) return;
            eventArgs.Building.SetOccupant(null);
        }

        public void CreateDeleteBuildingForOccupantEvent(Building building)
        {
            onDeleteBuildingForOccupantEvent.Raise(new BuildingEventArgs(building));
        }
    }
}