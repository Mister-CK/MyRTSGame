using System.Collections.Generic;
using UnityEngine;

namespace MyRTSGame.Model.ResourceSystem.Model
{
    public class NaturalResource: MonoBehaviour
    {
        protected Resource Resource;
        public BoxCollider BCollider { get; private set; }
        
        public Resource GetResource()
        {
            return Resource;
        }
        
        public void SetResource(Resource resource)
        {
            Resource = resource;
        }
    }
}