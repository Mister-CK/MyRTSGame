using UnityEngine;

namespace MyRTSGame.Model
{
    public class Villager : Unit
    {
        private readonly Resource _resource = new() { ResourceType = ResourceType.Stone, Quantity = 1 };
        private Job _currentJob;
        private Building _destination;
        private bool _hasDestination;
        private bool _hasResource;
        private JobQueue _jobQueue;

        private void Start()
        {
            _jobQueue = JobQueue.GetInstance();
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
            if (_hasResource)
                DeliverResource(_destination, _resource.ResourceType);
            else
                TakeResource(_destination, _resource.ResourceType);

            _hasDestination = false;
        }

        private void TakeResource(Building building, ResourceType resourceType)
        {
            _hasResource = true;
            building.BuildingController.RemoveResource(resourceType, 1);
        }

        private void DeliverResource(Building building, ResourceType resourceType)
        {
            foreach(var res in building.ResourcesInJobForBuilding)
            {
                if (res.ResourceType == resourceType) res.Quantity--;
            }
            _hasResource = false;
            building.BuildingController.AddResource(resourceType, 1);
        }

        private void PerformNextJob()
        {
            _currentJob = _jobQueue.GetNextJob();
            if (_currentJob == null) return;
            _destination = _currentJob.Origin;
            _resource.ResourceType = _currentJob.ResourceType;
            Agent.SetDestination(_destination.transform.position);
            _hasDestination = true;
        }

        private void SetDestination()
        {
            if (!_hasResource)
            {
                PerformNextJob();
                return;
            }
            _destination = _currentJob.Destination;
            Agent.SetDestination(_destination.transform.position);
            _hasDestination = true;
        }

        public Job GetCurrentJob()
        {
            return _currentJob;
        }
        
        public bool HasDestination()
        {
            return _hasDestination;
        }
    }
}