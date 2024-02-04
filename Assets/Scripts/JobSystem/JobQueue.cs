using System.Collections.Generic;

namespace MyRTSGame.Model
{
    public class JobQueue
    {
        private static JobQueue _instance;
        private readonly List<VillagerJob> _jobs;

        // Private constructor to prevent instantiation
        private JobQueue()
        {
            _jobs = new List<VillagerJob>();
        }

        public static JobQueue GetInstance()
        {
            return _instance ??= new JobQueue();
        }

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