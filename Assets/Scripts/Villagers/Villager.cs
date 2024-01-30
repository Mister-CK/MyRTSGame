using System;
using System.Collections.Generic;
using MyRTSGame.Interface;
using UnityEngine;
using UnityEngine.AI;

namespace MyRTSGame.Model
{
    public class Villager : MonoBehaviour, ISelectable
    {
        private SelectionManager _selectionManager;
        private BuildingList _buildingList;

        private NavMeshAgent _agent;
        private bool _hasDestination;
        private bool _hasResource = false;
        private readonly Resource _resource = new() { ResourceType = ResourceType.Stone, Quantity = 1};
        private Building _destination;
        private JobQueue _jobQueue;

        void Awake()
        {
            _selectionManager = SelectionManager.Instance;
            _agent = GetComponent<NavMeshAgent>();
            _jobQueue = JobQueue.GetInstance();
        }

        private void Start()
        {
            _buildingList = BuildingList.Instance;
        }

        void Update()
        {
            if (_hasDestination)
            {
                CheckIfDestinationIsReached();
            } else {
                SetDestination();
            }
        }

        public void HandleClick()
        {
            OnClick();
        }
        private void OnClick()
        {
            _selectionManager.SelectObject(this);
        }
        
        private void CheckIfDestinationIsReached()
        {
            if (_agent.pathPending) return;
            if (_agent.remainingDistance > _agent.stoppingDistance) return;
            if (_agent.hasPath || _agent.velocity.sqrMagnitude != 0f) return;
                
            if (_hasResource) {
                DeliverResource(_destination, _resource.ResourceType);
            } else {
                TakeResource(_destination, _resource.ResourceType);
            }
            
            _hasDestination = false;
        }

        private void TakeResource(Building building, ResourceType resourceType)
        {
            _hasResource = true;
            building.RemoveResource(resourceType, 1);
        }

        private void DeliverResource(Building building, ResourceType resourceType)
        {
            _hasResource = false;
            building.AddResource(resourceType, 1);
        }

        private void PerformNextJob()
        {
            var job = _jobQueue.GetNextJob();
            if (job == null) {
                return;
            }
            _destination = job.Destination;
            _resource.ResourceType = job.ResourceType;
            _agent.SetDestination(_destination.transform.position);
            _hasDestination = true;
        }

        private void SetDestination()
        {
            if (_hasResource)
            {
                PerformNextJob();
                return;
            }
            
            var buildings = _buildingList.GetBuildings();
            _destination = null; 
            Building warehouse = null;
            var resourceType = _resource.ResourceType;

            foreach (var building in buildings)
            {
                if (building.BuildingType == BuildingType.Warehouse)
                {
                    warehouse = building;
                    continue;
                }
                var inputTypes = building.InputTypes;
                var inventory = building.GetInventory();

                if (Array.IndexOf(inputTypes, resourceType) == -1)
                {
                    continue;
                }
                var resourceInInventory = Array.Find(inventory, res => res.ResourceType == resourceType);
                if (resourceInInventory != null && resourceInInventory.Quantity >= building.GetCapacity())
                {
                    continue;
                }
                _destination = building;
                break;
            }

            // If no suitable building is found, set destination to Warehouse
            if (_destination == null)
            {
                _destination = warehouse;
                if (_destination == null)
                {
                    throw new Exception("No Destination found");
                }
            }
            _agent.SetDestination(_destination.transform.position);
            _hasDestination = true;

        }
        
    }
}