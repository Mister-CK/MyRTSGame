using System.Collections.Generic;
using System.Linq;
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
        
        public List<NaturalResource> GetNaturalResourcesOfType(ResourceType resourceType)
        {
            return Resources[resourceType];
        }
        
        public void AddNaturalResource(NaturalResource newResource)
        {
            Resources[newResource.GetResource().ResourceType].Add(newResource);
        }
        
        public void RemoveResource(NaturalResource resourceToRemove)
        {
            Resources[resourceToRemove.GetResource().ResourceType].Remove(resourceToRemove);
        }
    }
}