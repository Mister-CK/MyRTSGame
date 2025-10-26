using Application.Factories;
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
        [SerializeField] private ResourceFactory resourceFactory;

        [SerializeField] private GameEvent onAddCollectResourceJobsEvent;
        [SerializeField] private GameEvent onSelectionEvent;
        
        public void CreateAddResourceJobsEvent(NaturalResource naturalResource)
        {
            onAddCollectResourceJobsEvent.Raise(new NaturalResourceEventArgs(naturalResource));
        }
        
        public void CreateNewJob(CollectResourceJob job)
        {
            if (job.Destination is not NaturalResource naturalResource) return;

            naturalResource.AddJobToDestination(job);
        }
        
        public void RemoveResourceFromDestination(IDestination destination, ResourceType resourceType, int quantity)
        {
            if (destination is not NaturalResource naturalResource) return;
            
            naturalResource.RemoveResource(resourceType, quantity);
        }

        public void PlantResource(PlantResourceJob job)
        {
            var plantedGameObject =  resourceFactory.CreateResource(job.Destination.GetPosition(), job.ResourceType);
            if (job.Destination is Terrain terrain)
            {
                terrain.SetHasResource(true);
                plantedGameObject.GetComponent<NaturalResource>().SetTerrain(terrain);
            }
        }
        
        public void HandleClick(ISelectable selectable)
        {
            onSelectionEvent.Raise(new SelectionEventArgs(selectable));
        }
    }
}