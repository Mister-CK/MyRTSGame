using MyRTSGame.Model.ResourceSystem.Controller;
using UnityEngine;

namespace MyRTSGame.Model.ResourceSystem.View
{
    public class NaturalResourceView: MonoBehaviour
    {
        protected ResourceController ResourceController;
        
        private void Start()
        {
            ResourceController = ResourceController.Instance;
        }
    }
}
