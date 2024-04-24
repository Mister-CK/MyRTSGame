
using System;
using MyRTSGame.Model.ResourceSystem.Model;
using UnityEngine;

namespace MyRTSGame.Model.ResourceSystem.Controller
{
    public class ResourceController :  MonoBehaviour
    {
        [SerializeField] private ResourceList _resourceList;

        [SerializeField] private GameEvent OnCollectResourceJobRequest;
        [SerializeField] private GameEvent OnCollectResourceAssignJob;
        [SerializeField] private GameEvent GetClosestResourceEvent;
        
        private void OnEnable()
        {
            GetClosestResourceEvent.RegisterListener(HandleGetClosestResourceEvent);
        }

        private void OnDisable()
        {
            GetClosestResourceEvent.UnregisterListener(HandleGetClosestResourceEvent);
        }

        private void HandleGetClosestResourceEvent(IGameEventArgs args)
        {
            if (args is not UnitWithJobEventArgs unitWithJobEventArgs) return;
            if (unitWithJobEventArgs.Unit is not ResourceCollector resourceCollector) return;
            var naturalResource = _resourceList.GetClosestResourceOfType(resourceCollector.GetResourceToCollect(), unitWithJobEventArgs.Unit.transform.position);
            //unitWithJobEventArgs.Job.Destination = naturalResource;

            //OnCollectResourceAssignJob.Raise();
        }
        
    }
}