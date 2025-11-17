using Enums;
using UnityEngine;

namespace Domain.Model
{
    [CreateAssetMenu(fileName = "LookingForBuildingJobQueue", menuName = "ScriptableObjects/LookingForBuildingJobQueue", order = 1)]
    public class LookingForBuildingJobQueue: JobQueueBase<LookingForBuildingJob>
    {
        public LookingForBuildingJob GetNextJobForUnitType(UnitType unitType)
        {
            return GetNextJobFor(job => job.UnitType == unitType);
        }
    }
}