
using System;
using MyRTSGame.Model.ResourceSystem.Model;
using UnityEditor;
using UnityEngine;

namespace MyRTSGame.Model.ResourceSystem.Controller
{
    public class ResourceController :  MonoBehaviour
    {
        [SerializeField] private GameEvent onResourceRemovedFromDestination;
        [SerializeField] private GameEvent onAddCollectResourceJobsEvent;
        [SerializeField] private GameEvent onNewJobCreated;
        [SerializeField] private GameEvent onSelectionEvent;
        
        [SerializeField] private ResourceList _resourceList;
        public static ResourceController Instance { get; private set; }

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
            onNewJobCreated.RegisterListener(HandleNewJobCreated);
            onResourceRemovedFromDestination.RegisterListener(HandleResourceRemovedFromDestination);
        }

        private void OnDisable()
        {
            onNewJobCreated.UnregisterListener(HandleNewJobCreated);
            onResourceRemovedFromDestination.UnregisterListener(HandleResourceRemovedFromDestination);
        }

        // private void HandleGetClosestResourceEvent(IGameEventArgs args)
        // {
        //     if (args is not UnitWithJobEventArgs unitWithJobEventArgs) return;
        //     if (unitWithJobEventArgs.Unit is not ResourceCollector resourceCollector) return;
        //     var naturalResource = _resourceList.GetClosestResourceOfType(resourceCollector.GetResourceToCollect(), unitWithJobEventArgs.Unit.transform.position);
        //     unitWithJobEventArgs.Job.Destination = naturalResource;
        //
        //     OnCollectResourceAssignJob.Raise();
        // }

        public void CreateAddResourceJobsEvent(NaturalResource naturalResource)
        {
            onAddCollectResourceJobsEvent.Raise(new NaturalResourceEventArgs(naturalResource));
        }
        
        private static void HandleNewJobCreated(IGameEventArgs args)
        {
            if (args is not JobEventArgs eventArgs) return;
            if (eventArgs.Job is not CollectResourceJob collectResourceJob) return;
            collectResourceJob.Destination.AddJobToDestination(eventArgs.Job);
        }
        
        private void HandleResourceRemovedFromDestination(IGameEventArgs args)
        {
            if (args is not DestinationResourceTypeQuantityEventArgs destinationResourceTypeQuantityEventArgs) return;
            if (destinationResourceTypeQuantityEventArgs.Destination is not NaturalResource naturalResource) return;
            
            naturalResource.RemoveResource(destinationResourceTypeQuantityEventArgs.ResourceType, destinationResourceTypeQuantityEventArgs.Quantity);
        }

        public void HandleClick(ISelectable selectable)
        {
            onSelectionEvent.Raise(new SelectionEventArgs(selectable));
        }
    }
}