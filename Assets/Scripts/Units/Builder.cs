using UnityEngine;
using UnityEngine.Serialization;

namespace MyRTSGame.Model
{
    public class Builder : Unit
    {
        [SerializeField] private BuilderJobQueue builderJobQueue;
        private BuilderJob _currentJob;
        private Building _destination;
        
        protected override void ExecuteJob()
        {
            _destination.SetState(new CompletedState(_destination.BuildingType));
            
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
            _destination = _currentJob.Destination;
            Agent.SetDestination(_destination.transform.position);
            HasDestination = true;
        }
        
    }
}