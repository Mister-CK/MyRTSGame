using System.Collections.Generic;

namespace MyRTSGame.Model
{
    public class JobQueue
    {
        private static JobQueue _instance;
        private readonly List<Job> _jobs;

        // Private constructor to prevent instantiation
        private JobQueue()
        {
            _jobs = new List<Job>();
        }

        public static JobQueue GetInstance()
        {
            return _instance ??= new JobQueue();
        }

        public void AddJob(Job job)
        {
            _jobs.Add(job);
        }

        public Job GetNextJob()
        {
            if (_jobs.Count <= 0) return null;

            var job = _jobs[0];
            _jobs.RemoveAt(0);
            return job;
        }

        public IEnumerable<Job> GetJobs()
        {
            return _jobs;
        }
    }
}