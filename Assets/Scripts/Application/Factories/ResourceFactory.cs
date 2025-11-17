using Domain.Model.ResourceSystem.Model;
using Enums;
using System;
using UnityEngine;

namespace Application.Factories
{
    public class ResourceFactory: MonoBehaviour
    {
        [SerializeField] private NaturalResource wheatPrefab;
        [SerializeField] private NaturalResource treePrefab;
        [SerializeField] private NaturalResource grapesPrefab;

        public NaturalResource CreateResource(Vector3 location, ResourceType resourceType)
        {
            NaturalResource resource;
            switch (resourceType)
            {
                case ResourceType.Lumber: 
                    resource = Instantiate(treePrefab, location, Quaternion.identity);
                    break;

                case ResourceType.Wheat: 
                    resource = Instantiate(wheatPrefab, location, Quaternion.identity);
                    break;
                case ResourceType.Wine: 
                    resource = Instantiate(grapesPrefab, location, Quaternion.identity);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Unkown resource type: " + resourceType);
            }
            if (ServiceInjector.Instance != null) ServiceInjector.Instance.InjectResourceDependencies(resource);
            return resource;
        }
    }
}
