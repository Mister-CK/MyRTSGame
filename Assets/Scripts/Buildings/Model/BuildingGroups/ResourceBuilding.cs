using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MyRTSGame.Model.ResourceSystem.Model;
using UnityEngine;

namespace MyRTSGame.Model
{
    public abstract class ResourceBuilding : Building
    {
        private List<CollectResourceJob> _collectResourceJobsForThisBuilding = new List<CollectResourceJob>();
        protected const float MaxDistanceFromBuilding = 10f;

        public IEnumerator CreateResource(int timeInSeconds, ResourceType resourceType)
        {
            while (true)
            {
                yield return new WaitForSeconds(timeInSeconds);
                if (Inventory[resourceType].Current < Capacity)
                {
                    AddResource(resourceType, 1);
                    buildingController.CreateJobNeededEvent(JobType.VillagerJob, null, this, resourceType, null);
                }
            }
        }

        private IEnumerable<NaturalResource> FindResourcesWithinRadius(ResourceType resourceType, float radius)
        {
            return Physics.OverlapSphere(transform.position, radius)
                .Select(hitCollider => hitCollider.GetComponentInParent<Tree>())
                .Where(naturalResource => naturalResource != null && naturalResource.GetResource().ResourceType == resourceType)
                .ToList();
        }
        
        public void AddCollectResourceJobsToBuilding(ResourceType resourceType)
        {
            _collectResourceJobsForThisBuilding.AddRange(
                FindResourcesWithinRadius(resourceType, MaxDistanceFromBuilding).Select(naturalResource => new CollectResourceJob
                {
                    Destination = naturalResource,
                    ResourceType = naturalResource.GetResource().ResourceType
                }).ToList()
            );
        }

        public CollectResourceJob GetCollectResourceJobFromBuilding()
        {
            if (_collectResourceJobsForThisBuilding.Count <= 0) return null;
            
            var job = _collectResourceJobsForThisBuilding[0];
            _collectResourceJobsForThisBuilding.RemoveAt(0);
            return job;
        }
        
    }
}