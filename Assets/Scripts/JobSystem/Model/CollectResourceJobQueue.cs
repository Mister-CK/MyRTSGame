using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyRTSGame.Model
{
    [CreateAssetMenu(fileName = "CollectResourceJobQueue", menuName = "ScriptableObjects/CollectResourceJobQueue", order = 1)]
    public class CollectResourceJobQueue : ScriptableObject
    {
        private Dictionary<ResourceType, List<CollectResourceJob>> _jobs =  new ();
        private IEnumerable<CollectResourceJob> _jobsInProgress = new List<CollectResourceJob>();

        public void AddJob(CollectResourceJob job)
        {
            Debug.Log("AddCollectResourceJob for ResourceType: " + job.ResourceType);
            if (!_jobs.ContainsKey(job.ResourceType))
            {
                _jobs[job.ResourceType] = new List<CollectResourceJob>();
            }
            _jobs[job.ResourceType].Add(job);
        }

        // public void RemoveJob(CollectResourceJob collectResourceJob)
        // {
        //     if (collectResourceJob.IsInProgress())
        //     {
        //         _jobsInProgress = _jobsInProgress.Where(job => job != collectResourceJob).ToList();
        //         return;
        //     }
        //     _jobs = _jobs.Where(job => job != collectResourceJob).ToList();
        // }
        
        public CollectResourceJob GetNextJobForResourceType(ResourceType resourceType)
        {
            Debug.Log("GetNextJobForResourceType for ResourceType: " + resourceType);
            if (_jobs[resourceType].Count <= 0) return null;

            var job = _jobs[resourceType][0];
            _jobs[resourceType].RemoveAt(0);
            return job;
        }

        // public IEnumerable<CollectResourceJob> GetJobs()
        // {
        //     return _jobs;
        // }
    }
}