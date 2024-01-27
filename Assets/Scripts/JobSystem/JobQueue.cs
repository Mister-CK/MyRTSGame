using System.Collections.Generic;

namespace MyRTSGame.Model
{
    public class JobQueue
    {
        // Singleton instance
        private static JobQueue instance;

        // List of jobs
        private List<Job> jobs;

        // Private constructor to prevent instantiation
        private JobQueue()
        {
            jobs = new List<Job>();
        }

        // Public method to get the singleton instance
        public static JobQueue GetInstance()
        {
            if (instance == null)
            {
                instance = new JobQueue();
            }

            return instance;
        }

        // Method to add a job to the queue
        public void AddJob(Job job)
        {
            jobs.Add(job);
        }

        // Method to get the next job from the queue
        public Job GetNextJob()
        {
            if (jobs.Count > 0)
            {
                Job job = jobs[0];
                jobs.RemoveAt(0);
                return job;
            }

            return null;
        }
    }

}