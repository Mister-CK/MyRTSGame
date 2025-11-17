using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Domain.Model
{
    public class JobQueueBase<T> : ScriptableObject where T : Job
    {
        protected List<T> Jobs = new();
        protected IEnumerable<T> JobsInProgress = new List<T>();

        public void AddJob(T job)
        {
            Jobs.Add(job);
        }

        public void RemoveJob(T job)
        {
            if (job.IsInProgress())
            {
                JobsInProgress = JobsInProgress.Where(j => j != job).ToList();
                return;
            }

            Jobs = Jobs.Where(j => j != job).ToList();
        }

        public T GetNextJob()
        {
            if (Jobs.Count <= 0) return null;

            var job = Jobs[0];
            Jobs.RemoveAt(0);
            JobsInProgress = JobsInProgress.Append(job);
            return job;
        }

        public IEnumerable<T> GetJobs()
        {
            return Jobs;
        }
        
        protected T GetNextJobFor(Func<T, bool> predicate)
        {
            var job = Jobs.FirstOrDefault(predicate);

            if (job != null)
            {
                Jobs.Remove(job);
                JobsInProgress = JobsInProgress.Append(job);
            }

            return job;
        }
    }
}