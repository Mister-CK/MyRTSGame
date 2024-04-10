using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyRTSGame.Model
{
    [CreateAssetMenu(fileName = "VillagerJobQueue", menuName = "ScriptableObjects/VillagerJobQueue", order = 1)]
    public class VillagerJobQueue :  ScriptableObject
    {
        private List<VillagerJob> _jobs = new();
        private IEnumerable<VillagerJob> _jobsInProgress = new List<VillagerJob>();
        public void AddJob(VillagerJob villagerJob)
        {
            _jobs.Add(villagerJob);
        }

        public VillagerJob GetNextJob()
        {
            if (_jobs.Count <= 0) return null;

            var job = _jobs[0];
            _jobs.RemoveAt(0);
            _jobsInProgress = _jobsInProgress.Append(job);
            return job;
        }

        public IEnumerable<VillagerJob> GetJobs()
        {
            return _jobs;
        }
    }
}