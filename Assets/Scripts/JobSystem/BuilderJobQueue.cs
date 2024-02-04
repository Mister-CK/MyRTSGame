using System.Collections.Generic;

namespace MyRTSGame.Model
{
    public class BuilderJobQueue
    {
        private static BuilderJobQueue _instance;
        private readonly List<BuilderJob> _builderJobs;

        
        // Private constructor to prevent instantiation
        private BuilderJobQueue()
        {
            _builderJobs = new List<BuilderJob>();
        }

        public static BuilderJobQueue GetInstance()
        {
            return _instance ??= new BuilderJobQueue();
        }
        
        public void AddJob(BuilderJob job)
        {
            _builderJobs.Add(job);
        }

        public BuilderJob GetNextJob()
        {
            if (_builderJobs.Count <= 0) return null;

            var job = _builderJobs[0];
            _builderJobs.RemoveAt(0);
            return job;
        }

        public IEnumerable<BuilderJob> GetJobs()
        {
            return _builderJobs;
        }
    }
}