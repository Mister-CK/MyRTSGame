using System.Collections.Generic;
using System.Linq;
using MyRTSGame.Model.ResourceSystem.Controller;
using UnityEngine;

namespace MyRTSGame.Model.ResourceSystem.Model
{
    public class ResourceList: MonoBehaviour
    {
        Dictionary<ResourceType, List<NaturalResource>> Resources = new Dictionary<ResourceType, List<NaturalResource>>();
        
        private List<ResourceType> naturalResourceTypes = new List<ResourceType>() { ResourceType.Stone , ResourceType.Wood};

        public void Awake()
        {
            foreach (var resourceType in naturalResourceTypes)
            {
                Resources.Add(resourceType, FindObjectsOfType<NaturalResource>().ToList());
            }
        }
        
        // public NaturalResource GetClosestResourceOfType(ResourceType resourceType, Vector3 position)
        // {
        //     var resources = Resources[resourceType];
        //     NaturalResource closestResource = null;
        //     var closestDistance = float.MaxValue;
        //     foreach (var resource in resources)
        //     {
        //         var distance = Vector3.Distance(resource.transform.position, position);
        //         if (distance >= closestDistance) continue;
        //         closestDistance = distance;
        //         closestResource = resource;
        //     }
        //
        //     return closestResource;
        // }
        
        // public List<NaturalResource> GetNaturalResourcesOfType(ResourceType resourceType)
        // {
        //     return Resources[resourceType];
        // }
        //
        // public void AddNaturalResource(NaturalResource newResource)
        // {
        //     Resources[newResource.GetResource().ResourceType].Add(newResource);
        // }
        //
        // public void RemoveResource(NaturalResource resourceToRemove)
        // {
        //     Resources[resourceToRemove.GetResource().ResourceType].Remove(resourceToRemove);
        // }
    }
}