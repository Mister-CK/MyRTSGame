
using System;
using MyRTSGame.Model.ResourceSystem.Model;
using UnityEditor;
using UnityEngine;

namespace MyRTSGame.Model.ResourceSystem.Controller
{
    public class ResourceController :  MonoBehaviour
    {
        [SerializeField] private ResourceList _resourceList;
        
        [SerializeField] private GameEvent onAddCollectResourceJobsEvent;

        public static ResourceController Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }
        private void OnEnable()
        {
        }

        private void OnDisable()
        {
        }

        // private void HandleGetClosestResourceEvent(IGameEventArgs args)
        // {
        //     if (args is not UnitWithJobEventArgs unitWithJobEventArgs) return;
        //     if (unitWithJobEventArgs.Unit is not ResourceCollector resourceCollector) return;
        //     var naturalResource = _resourceList.GetClosestResourceOfType(resourceCollector.GetResourceToCollect(), unitWithJobEventArgs.Unit.transform.position);
        //     unitWithJobEventArgs.Job.Destination = naturalResource;
        //
        //     OnCollectResourceAssignJob.Raise();
        // }

        public void CreateAddResourceJobsEvent(NaturalResource naturalResource)
        {
            onAddCollectResourceJobsEvent.Raise(new NaturalResourceEventArgs(naturalResource));
        }
        
    }
}