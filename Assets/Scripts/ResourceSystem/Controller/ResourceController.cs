
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
        [SerializeField] private GameEvent onPlantResourceEvent;
        [SerializeField] private ResourceList _resourceList;
        [SerializeField] private GameObject treePrefab;
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
            onPlantResourceEvent.RegisterListener(HandleOnPlantResourceEvent);
        }

        private void OnDisable()
        {
            onNewJobCreated.UnregisterListener(HandleNewJobCreated);
            onResourceRemovedFromDestination.UnregisterListener(HandleResourceRemovedFromDestination);
            onPlantResourceEvent.UnregisterListener(HandleOnPlantResourceEvent);
        }

        public void CreateAddResourceJobsEvent(NaturalResource naturalResource)
        {
            onAddCollectResourceJobsEvent.Raise(new NaturalResourceEventArgs(naturalResource));
        }
        
        private static void HandleNewJobCreated(IGameEventArgs args)
        {
            if (args is not JobEventArgs eventArgs) return;
            if (eventArgs.Job is not CollectResourceJob collectResourceJob) return;
            if (collectResourceJob.Destination is not NaturalResource naturalResource) return;
            naturalResource.AddJobToDestination(eventArgs.Job);
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
        
        private void PlantTree(Vector3 location)
        {
            Instantiate(treePrefab, location, Quaternion.identity);
        }

        private void HandleOnPlantResourceEvent(IGameEventArgs args)
        {
            if (args is not JobEventArgs jobEventArgs) return;
            if (jobEventArgs.Job is not PlantResourceJob plantResourceJob) return;
            if (plantResourceJob.ResourceType == ResourceType.Lumber) PlantTree(plantResourceJob.Destination.GetPosition());
        }
    }
}