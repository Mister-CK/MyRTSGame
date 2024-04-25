using UnityEngine;

namespace MyRTSGame.Model
{
    [CreateAssetMenu(fileName = "ConsumptionJobQueue", menuName = "ScriptableObjects/ConsumptionJobQueue", order = 1)]
    public class ConsumptionJobQueue: JobQueueBase<ConsumptionJob>
    {
    }
}