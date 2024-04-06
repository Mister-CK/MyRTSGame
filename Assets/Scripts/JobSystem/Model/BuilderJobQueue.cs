using System.Collections.Generic;
using UnityEngine;

namespace MyRTSGame.Model
{
    [CreateAssetMenu(fileName = "BuilderJobQueue", menuName = "ScriptableObjects/BuilderJobQueue", order = 1)]
    public class BuilderJobQueue : ScriptableObject
    {
        private readonly List<BuilderJob> _builderJobs =  new ();

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