using UnityEngine;
using UnityEngine.Serialization;

namespace MyRTSGame.Model
{
    public class Builder : Unit
    {
        [SerializeField] private BuilderJobQueue builderJobQueue;
        private BuilderJob _currentJob;
        
        protected override void ExecuteJob()
        {
            Destination.SetState(new CompletedState(Destination.BuildingType));
            
            HasDestination = false;
        }
        
        protected override void SetDestination()
        {
            PerformNextJob();
        }

        private void PerformNextJob()
        {
            _currentJob = builderJobQueue.GetNextJob();
            if (_currentJob == null) return;
            Destination = _currentJob.Destination;
            Agent.SetDestination(Destination.transform.position);
            HasDestination = true;
        }
        
    }
}