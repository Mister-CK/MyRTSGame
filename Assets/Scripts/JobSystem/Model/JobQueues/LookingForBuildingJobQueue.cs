using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyRTSGame.Model
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