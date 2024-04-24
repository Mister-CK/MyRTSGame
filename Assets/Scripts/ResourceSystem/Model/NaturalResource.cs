using System;
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
        public Resource GetResource()
        {
            return Resource;
        }

        private void Awake()
        {
            ResourceController = ResourceController.Instance;
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
            throw new System.NotImplementedException();
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