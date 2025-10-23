using Enums;
using Interface;
using MyRTSGame.Model;
using System;
using MyRTSGame.Model.ResourceSystem.Model;
using UnityEngine;
using Terrain = Terrains.Model.Terrain;

namespace Application
{
    public class ResourceService :  MonoBehaviour
    {
        [SerializeField] private GameEvent onResourceRemovedFromDestination;
        [SerializeField] private GameEvent onAddCollectResourceJobsEvent;
        [SerializeField] private GameEvent onNewJobCreated;
        [SerializeField] private GameEvent onSelectionEvent;
        [SerializeField] private GameEvent onPlantResourceEvent;
        [SerializeField] private GameObject wheatPrefab;
        [SerializeField] private GameObject treePrefab;
        [SerializeField] private GameObject grapesPrefab;
        
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
        
        private GameObject PlantResource(Vector3 location, ResourceType resourceType)
        {
            if (resourceType == ResourceType.Lumber) return Instantiate(treePrefab, location, Quaternion.identity);
            if (resourceType == ResourceType.Wheat) return Instantiate(wheatPrefab, location, Quaternion.identity);
            if (resourceType == ResourceType.Wine) return Instantiate(grapesPrefab, location, Quaternion.identity);
            throw new ArgumentOutOfRangeException("Unkown resource type: " + resourceType);
        }


        private void HandleOnPlantResourceEvent(IGameEventArgs args)
        {
            if (args is not JobEventArgs jobEventArgs) return;
            if (jobEventArgs.Job is not PlantResourceJob plantResourceJob) return;
            var plantedGameObject = PlantResource(plantResourceJob.Destination.GetPosition(), plantResourceJob.ResourceType);
            if (jobEventArgs.Job.Destination is Terrain terrain)
            {
                terrain.SetHasResource(true);
                plantedGameObject.GetComponent<NaturalResource>().SetTerrain(terrain);
            }
        }
    }
}