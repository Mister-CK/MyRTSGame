using UnityEngine;

namespace MyRTSGame.Model
{
    [CreateAssetMenu(fileName = "BuilderJobQueue", menuName = "ScriptableObjects/BuilderJobQueue", order = 1)]
    public class BuilderJobQueue : JobQueueBase<BuilderJob>
    {
    }
}