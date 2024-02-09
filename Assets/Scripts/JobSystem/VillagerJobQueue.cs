using System.Collections.Generic;
using UnityEngine;

namespace MyRTSGame.Model
{
    [CreateAssetMenu(fileName = "VillagerJobQueue", menuName = "ScriptableObjects/VillagerJobQueue", order = 1)]
    public class VillagerJobQueue :  ScriptableObject
    {
        private readonly List<VillagerJob> _jobs = new();

        public void AddJob(VillagerJob villagerJob)
        {
            _jobs.Add(villagerJob);
        }

        public VillagerJob GetNextJob()
        {
            if (_jobs.Count <= 0) return null;

            var job = _jobs[0];
            _jobs.RemoveAt(0);
            return job;
        }

        public IEnumerable<VillagerJob> GetJobs()
        {
            return _jobs;
        }
    }
}