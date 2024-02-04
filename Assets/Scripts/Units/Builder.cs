using UnityEngine;

namespace MyRTSGame.Model
{
    public class Builder : Unit
    {
        private BuilderJobQueue _builderJobQueue;
        private BuilderJob _currentJob;
        private Building _destination;
        private void Start()
        {
            _builderJobQueue = BuilderJobQueue.GetInstance();
            SelectionManager = SelectionManager.Instance;
        }
        
        protected override void ExcecuteJob()
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
            _currentJob = _builderJobQueue.GetNextJob();
            if (_currentJob == null) return;
            _destination = _currentJob.Destination;
            Agent.SetDestination(_destination.transform.position);
            HasDestination = true;
        }
        
    }
}