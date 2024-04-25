
using System.Linq;
using UnityEngine;

namespace MyRTSGame.Model
{
    [CreateAssetMenu(fileName = "CollectResourceJobQueue", menuName = "ScriptableObjects/CollectResourceJobQueue", order = 1)]
    public class CollectResourceJobQueue : JobQueueBase<CollectResourceJob>
    {
        public CollectResourceJob GetNextJobForResourceType(ResourceType resourceType)
        {
            return GetNextJobFor(job => job.ResourceType == resourceType);
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
        
        // public IEnumerable<CollectResourceJob> GetJobs()
        // {
        //     return _jobs;
        // }
    }
}