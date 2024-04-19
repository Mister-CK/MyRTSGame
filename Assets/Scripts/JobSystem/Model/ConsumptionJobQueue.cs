using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyRTSGame.Model
{
    [CreateAssetMenu(fileName = "ConsumptionJobQueue", menuName = "ScriptableObjects/ConsumptionJobQueue", order = 1)]
    public class ConsumptionJobQueue: ScriptableObject
    {
        private List<ConsumptionJob> _jobs = new();
        private IEnumerable<ConsumptionJob> _jobsInProgress = new List<ConsumptionJob>();

        public void AddJob(ConsumptionJob consumptionJob)
        {
            _jobs.Add(consumptionJob);
        }

        public void RemoveJob(ConsumptionJob consumptionJob)
        {
            if (consumptionJob.IsInProgress())
            {
                _jobsInProgress = _jobsInProgress.Where(job => job != consumptionJob).ToList();
                return;
            }

            _jobs = _jobs.Where(job => job != consumptionJob).ToList();
        }

        public ConsumptionJob GetNextJob()
        {
            if (_jobs.Count <= 0) return null;

            var job = _jobs[0];
            _jobs.RemoveAt(0);
            _jobsInProgress = _jobsInProgress.Append(job);
            return job;
        }

        public IEnumerable<ConsumptionJob> GetJobs()
        {
            return _jobs;
        }
    }
}