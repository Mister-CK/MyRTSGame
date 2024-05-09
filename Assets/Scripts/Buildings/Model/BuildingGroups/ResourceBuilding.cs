using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MyRTSGame.Model.ResourceSystem.Model;
using Unity.VisualScripting;
using UnityEngine;

namespace MyRTSGame.Model
{
    public abstract class ResourceBuilding : Building
    {
        private List<CollectResourceJob> _collectResourceJobsForThisBuilding = new List<CollectResourceJob>();
        protected const float MaxDistanceFromBuilding = 10f;

        public float GetMaxDistanceFromBuilding()
        {
            return MaxDistanceFromBuilding;
        }
        
        public IEnumerator CreateResource(int timeInSeconds, ResourceType resourceType)
        {
            while (true)
            {
                yield return new WaitForSeconds(timeInSeconds);
                if (Inventory[resourceType].Current < Capacity)
                {
                    ModifyInventory(resourceType, data => data.Current++);
                    BuildingController.CreateJobNeededEvent(JobType.VillagerJob, null, this, resourceType, null);
                }
            }
        }

        private NaturalResource FindAvailableResourcesWithinRadius(ResourceType resourceType, float radius)
        {
            return Physics.OverlapSphere(transform.position, radius)
                .Select(hitCollider => hitCollider.GetComponentInParent<Tree>())
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