using Application.Services;
using Interface;
using Domain;
using Domain.Model;
using UnityEngine;

namespace Application.EventHandlers
{
    public class ResourceEventsHandler: MonoBehaviour
    {
        [SerializeField] private ResourceService resourceService;
        [SerializeField] private GameEvent onNewJobCreated;
        [SerializeField] private GameEvent onPlantResourceEvent;
        [SerializeField] private GameEvent onResourceRemovedFromDestination;

        private void OnEnable()
        {
            onNewJobCreated.RegisterListener(HandleNewJobCreated);
            onResourceRemovedFromDestination.RegisterListener(HandleResourceRemovedFromDestination);
            onPlantResourceEvent.RegisterListener(HandleOnPlantResourceEvent);
        }

        private void OnDisable()
        {
            onNewJobCreated.UnregisterListener(HandleNewJobCreated);
            onResourceRemovedFromDestination.UnregisterListener(HandleResourceRemovedFromDestination);
            onPlantResourceEvent.UnregisterListener(HandleOnPlantResourceEvent);
        }
        
        private void HandleNewJobCreated(IGameEventArgs args)
        {
            if (args is not JobEventArgs eventArgs) return;
            if (eventArgs.Job is not CollectResourceJob collectResourceJob) return;
            resourceService.CreateNewJob(collectResourceJob);
        }

        private void HandleResourceRemovedFromDestination(IGameEventArgs args)
        {
            if (args is not DestinationResourceTypeQuantityEventArgs destinationResourceTypeQuantityEventArgs) return;
            resourceService.RemoveResourceFromDestination(destinationResourceTypeQuantityEventArgs.Destination, destinationResourceTypeQuantityEventArgs.ResourceType, destinationResourceTypeQuantityEventArgs.Quantity);
        }
        
        private void HandleOnPlantResourceEvent(IGameEventArgs args)
        {
            if (args is not JobEventArgs jobEventArgs) return;
            if (jobEventArgs.Job is not PlantResourceJob plantResourceJob) return;
            resourceService.PlantResource(plantResourceJob);
        }

    }
}
