using Application.Services;
using Buildings.Model;
using Interface;
using UnityEngine;

namespace Application.EventHandlers
{
    public class BuildingEventsHandler: MonoBehaviour
    {
        [SerializeField] private BuildingService buildingService;
        
        [SerializeField] private GameEvent onResourceRemovedFromDestination;
        [SerializeField] private GameEvent onResourceAddedToBuilding;
        [SerializeField] private GameEvent onAddProductionJobEvent;
        [SerializeField] private GameEvent onRemoveProductionJobEvent;
        [SerializeField] private GameEvent onAddTrainingJobEvent;
        [SerializeField] private GameEvent onRemoveTrainingJobEvent;
        [SerializeField] private GameEvent onDeleteBuildingEvent; 
        [SerializeField] private GameEvent onNewJobCreated;
        [SerializeField] private GameEvent onDeleteJobEvent;
        [SerializeField] private GameEvent onCompleteJobEvent;
        [SerializeField] private GameEvent onRemoveOccupantFromBuildingEvent;

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
        
         private void OnResourceAdded(IGameEventArgs args)
        {
            if (args is not DestinationResourceTypeQuantityEventArgs eventArgs) return;
            if (eventArgs.Destination is not Building building) return;
            buildingService.AddResource(building, eventArgs.ResourceType, eventArgs.Quantity);
        }
        
        private void OnResourceRemoved(IGameEventArgs args)
        {
            if (args is not DestinationResourceTypeQuantityEventArgs eventArgs) return;
            if (eventArgs.Destination is not Building building) return;
            buildingService.RemoveResource(building, eventArgs.ResourceType, eventArgs.Quantity);
        }
        
        private void OnAddProductionJob(IGameEventArgs args)
        {
            if (args is not WorkshopBuildingBuildingResourceTypeEventArgs eventArgs) return;
            Debug.Log("Adding production job");
            buildingService.AddProductionJob(eventArgs.WorkshopBuilding, eventArgs.ResourceType);
        }
        
        private void OnRemoveProductionJob(IGameEventArgs args)
        {
            if (args is not WorkshopBuildingBuildingResourceTypeEventArgs eventArgs) return;
            buildingService.RemoveProductionJob(eventArgs.WorkshopBuilding, eventArgs.ResourceType);
        }
        
        private void OnAddTrainingJob(IGameEventArgs args)
        {
            if (args is not TrainingBuildingUnitTypeEventArgs eventArgs) return;
            buildingService.AddTrainingJob(eventArgs.TrainingBuilding, eventArgs.UnitType);
        }
        
        private void OnRemoveTrainingJob(IGameEventArgs args)
        {
            if (args is not TrainingBuildingUnitTypeEventArgs eventArgs) return;
            buildingService.RemoveTrainingJob(eventArgs.TrainingBuilding, eventArgs.UnitType);
        }

        private void HandleDeleteBuildingEvent(IGameEventArgs args)
        {
            if (args is not BuildingEventArgs eventArgs) return; 
            buildingService.DeleteBuilding(eventArgs.Building);
        }
        
        private void HandleOnRemoveOccupantFromBuildingEvent(IGameEventArgs args)
        {
            if (args is not BuildingEventArgs eventArgs) return;
            buildingService.RemoveOccupantFromBuilding(eventArgs.Building);
        }

        private void HandleNewJobCreated(IGameEventArgs args)
        {
            if (args is not JobEventArgs eventArgs) return;
            buildingService.CreateNewJob(eventArgs.Job);
        }
        
        private void HandleDeleteJobEvent(IGameEventArgs args)
        {
            if (args is not JobEventArgs eventArgs) return;
            buildingService.DeleteJob(eventArgs.Job);
        }

        private void HandleCompleteJobEvent(IGameEventArgs args)
        {
            if (args is not JobEventArgs eventArgs) return;
            buildingService.CompleteJob(eventArgs.Job);
        }
    }
}
