using UnityEngine;

namespace Domain.Model
{
    [CreateAssetMenu(fileName = "VillagerJobQueue", menuName = "ScriptableObjects/VillagerJobQueue", order = 1)]
    public class VillagerJobQueue :  JobQueueBase<VillagerJob>
    {
    }
}