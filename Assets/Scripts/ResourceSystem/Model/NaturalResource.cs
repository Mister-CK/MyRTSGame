using System.Collections.Generic;
using MyRTSGame.Model.ResourceSystem.Controller;
using UnityEngine;

namespace MyRTSGame.Model.ResourceSystem.Model
{
    public class NaturalResource: MonoBehaviour, IDestination
    {
        protected Resource Resource;
        public BoxCollider BCollider { get; private set; }
        protected ResourceController ResourceController;
        private List<CollectResourceJob> _collectResourceJobs = new List<CollectResourceJob>();

        protected virtual void Start()
        {
            ResourceController = ResourceController.Instance;
        }
        
        public Resource GetResource()
        {
            return Resource;
        }
        public void SetResource(Resource resource)
        {
            Resource = resource;
        }
        
        
        public Vector3 GetPosition()
        {
            return transform.position;
        }
        
        public void AddJobToDestination(Job job)
        {
            if (job is not CollectResourceJob collectResourceJob) return;
            _collectResourceJobs.Add(collectResourceJob);
        }

        public BuildingType GetBuildingType()
        {
            throw new System.NotImplementedException();
        }   
        public void SetState(IBuildingState newState)
        {
            throw new System.NotImplementedException();
        }
    }
}