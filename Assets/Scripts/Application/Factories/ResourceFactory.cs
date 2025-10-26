using Enums;
using System;
using UnityEngine;

namespace Application.Factories
{
    public class ResourceFactory: MonoBehaviour
    {
        [SerializeField] private GameObject wheatPrefab;
        [SerializeField] private GameObject treePrefab;
        [SerializeField] private GameObject grapesPrefab;

        public GameObject CreateResource(Vector3 location, ResourceType resourceType)
        {
            if (resourceType == ResourceType.Lumber) return Instantiate(treePrefab, location, Quaternion.identity);
            if (resourceType == ResourceType.Wheat) return Instantiate(wheatPrefab, location, Quaternion.identity);
            if (resourceType == ResourceType.Wine) return Instantiate(grapesPrefab, location, Quaternion.identity);
            throw new ArgumentOutOfRangeException("Unkown resource type: " + resourceType);
        }
    }
}
