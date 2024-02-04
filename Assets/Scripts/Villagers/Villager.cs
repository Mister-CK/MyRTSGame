using System;
using UnityEngine;
using UnityEngine.AI;

namespace MyRTSGame.Model
{
    public class Villager : MonoBehaviour, ISelectable
    {
        private readonly Resource _resource = new() { ResourceType = ResourceType.Stone, Quantity = 1 };
        private NavMeshAgent _agent;
        private Job _currentJob;
        private Building _destination;
        private bool _hasDestination;
        private bool _hasResource;
        private JobQueue _jobQueue;
        private SelectionManager _selectionManager;

        private void Awake()
        {
            _agent = GetComponentInChildren<NavMeshAgent>();
        }

        private void Start()
        {
            _jobQueue = JobQueue.GetInstance();
            _selectionManager = SelectionManager.Instance;
        }

        private void Update()
        {
            if (_hasDestination)
                CheckIfDestinationIsReached();
            else
                SetDestination();
        }

        public void HandleClick()
        {
            _selectionManager.SelectObject(this);
        }

        private void CheckIfDestinationIsReached()
        {
            if (_agent.pathPending)
            {
                return;
            }
            if (_agent.remainingDistance > _agent.stoppingDistance +  0.5f)
            {
                return;
            }
            if (_agent.hasPath || _agent.velocity.sqrMagnitude != 0f)
            {
                // If the agent is not moving but still has a path, clear the path
                if (_agent.velocity.sqrMagnitude == 0f && _agent.hasPath)
                {
                    Debug.Log("Agent is frozen but still has a path. Clearing path.");
                    _agent.ResetPath();
                }
                
                return;
            }

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
            _agent.SetDestination(_destination.transform.position);
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
            _agent.SetDestination(_destination.transform.position);
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