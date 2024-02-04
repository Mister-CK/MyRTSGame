using UnityEngine;

namespace MyRTSGame.Model
{
    public class Builder : Unit
    {
        private BuilderJobQueue _builderJobQueue;
        private BuilderJob _currentJob;
        private Building _destination;
        private bool _hasDestination;
        private void Start()
        {
            _builderJobQueue = BuilderJobQueue.GetInstance();
            SelectionManager = SelectionManager.Instance;
        }
        
        private void Update()
        {
            if (_hasDestination)
                CheckIfDestinationIsReached();
            else
                SetDestination();
        }
        
        private void CheckIfDestinationIsReached()
        {
            if (Agent.pathPending)
            {
                return;
            }
            if (Agent.remainingDistance > Agent.stoppingDistance +  0.5f)
            {
                return;
            }
            if (Agent.hasPath || Agent.velocity.sqrMagnitude != 0f)
            {
                // If the agent is not moving but still has a path, clear the path
                if (Agent.velocity.sqrMagnitude == 0f && Agent.hasPath)
                {
                    Debug.Log("Agent is frozen but still has a path. Clearing path.");
                    Agent.ResetPath();
                }
                
                return;
            }

            //Destination reached
            _destination.SetState(new CompletedState(_destination.BuildingType));
            
            _hasDestination = false;
        }
        
        private void SetDestination()
        {
            PerformNextJob();
        }

        private void PerformNextJob()
        {
            _currentJob = _builderJobQueue.GetNextJob();
            if (_currentJob == null) return;
            _destination = _currentJob.Destination;
            Agent.SetDestination(_destination.transform.position);
            _hasDestination = true;
        }
        
    }
}