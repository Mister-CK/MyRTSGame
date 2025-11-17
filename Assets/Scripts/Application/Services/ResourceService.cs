using Application.Factories;
using Enums;
using Interface;
using Domain.Model;
using Domain.Model.ResourceSystem.Model;
using UnityEngine;
using Terrain = Terrains.Model.Terrain;

namespace Application.Services
{
    public class ResourceService :  MonoBehaviour
    {
        [SerializeField] private ResourceFactory resourceFactory;

        [SerializeField] private GameEvent onAddCollectResourceJobsEvent;
        [SerializeField] private GameEvent onSelectionEvent;
        [SerializeField] private GameEvent onDeselectionEvent;
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
            var plantedGameObject =  resourceFactory.CreateResource(job.Destination.GetPosition(), job.ResourceType).gameObject;
            if (job.Destination is not Terrain terrain) return;
            terrain.SetHasResource(true);
            plantedGameObject.GetComponent<NaturalResource>().SetTerrain(terrain);
        }
        
        public void HandleClick(ISelectable selectable)
        {
            onSelectionEvent.Raise(new SelectionEventArgs(selectable));
        }
        public void HandleDeselect()
        {
            onDeselectionEvent.Raise(null);
        }
    }
}