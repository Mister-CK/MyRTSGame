using Enums;
using Domain.Model;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Domain.Model.ResourceSystem.Model;
using UnityEngine;

namespace Buildings.Model.BuildingGroups
{
    public abstract class ResourceBuilding : Building
    {
        private List<CollectResourceJob> _collectResourceJobsForThisBuilding = new List<CollectResourceJob>();
        private const float MaxDistanceFromBuilding = 10f;

        public float GetMaxDistanceFromBuilding()
        {
            return MaxDistanceFromBuilding;
        }
        
        protected IEnumerator CreateResource(int timeInSeconds, ResourceType resourceType)
        {
            while (true)
            {
                yield return new WaitForSeconds(timeInSeconds);
                if (Inventory[resourceType].Current < Capacity)
                {
                    ModifyInventory(resourceType, data => data.Current++);
                    BuildingService.CreateJobNeededEvent(JobType.VillagerJob, null, this, resourceType, null);
                }
            }
        }

        private NaturalResource FindAvailableResourcesWithinRadius(ResourceType resourceType, float radius)
        {
            return Physics.OverlapSphere(transform.position, radius)
                .Select(hitCollider => hitCollider.GetComponentInParent<NaturalResource>())
                .FirstOrDefault(naturalResource => naturalResource != null &&
                                                   naturalResource.GetResourceType() == resourceType &&
                                                   naturalResource.GetInventory()[resourceType].Current >
                                                   naturalResource.GetInventory()[resourceType].Outgoing);
        }
        
        public CollectResourceJob GetCollectResourceJob(ResourceType resourceType)
        {
            var naturalResource = FindAvailableResourcesWithinRadius(resourceType, MaxDistanceFromBuilding);
            if (naturalResource == null) return null;
            var collectResourceJob = new CollectResourceJob()
            {
                Destination = naturalResource,
                ResourceType = naturalResource.GetResourceType()
            };
            naturalResource.ModifyInventory(resourceType, data => data.Outgoing++);
            return collectResourceJob;
        }

        public void AddCollectResourceJobToBuilding(CollectResourceJob collectResourceJob)
        {
            _collectResourceJobsForThisBuilding.Add(collectResourceJob);
        }
    }
}