using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyRTSGame.Model
{
    [CreateAssetMenu(fileName = "LookingForBuildingJobQueue", menuName = "ScriptableObjects/LookingForBuildingJobQueue", order = 1)]
    public class LookingForBuildingJobQueue: ScriptableObject
    {
        private List<LookingForBuildingJob> _jobs = new();
        private IEnumerable<LookingForBuildingJob> _jobsInProgress = new List<LookingForBuildingJob>();

        public void AddJob(LookingForBuildingJob lookingForBuildingJob)
        {
            _jobs.Add(lookingForBuildingJob);
        }

        public void RemoveJob(LookingForBuildingJob lookingForBuildingJob)
        {
            if (lookingForBuildingJob.IsInProgress())
            {
                _jobsInProgress = _jobsInProgress.Where(job => job != lookingForBuildingJob).ToList();
                return;
            }

            _jobs = _jobs.Where(job => job != lookingForBuildingJob).ToList();
        }

        public LookingForBuildingJob GetNextJobForUnitType(UnitType unitType)
        {
            var firstJobForUnit = _jobs.FirstOrDefault(job => job.UnitType == unitType);

            if (firstJobForUnit != null)
            {
                _jobs.Remove(firstJobForUnit);
                _jobsInProgress = _jobsInProgress.Append(firstJobForUnit);
            }

            return firstJobForUnit;
        }
        public IEnumerable<LookingForBuildingJob> GetJobs()
        {
            return _jobs;
        }
    }
}