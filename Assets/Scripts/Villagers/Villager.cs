using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MyRTSGame.Model
{
    public class Villager : MonoBehaviour
    {

        private BuildingList _buildingList;

        private NavMeshAgent _agent;
        private bool hasDestination;
        private bool hasResource = false;
        private Resource resource = new() { ResourceType = ResourceType.Stone, Quantity = 1};
        private Building destination;
        private JobQueue jobQueue;

        void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            jobQueue = JobQueue.GetInstance();
        }

        private void Start()
        {
            _buildingList = BuildingList.Instance;
        }

        void Update()
        {
            if (!hasDestination) {
                SetDestination();
            } else {
                CheckIfDestinationIsReached();
            }
        }

        private void CheckIfDestinationIsReached()
        {
            if (!_agent.pathPending)
            {
                if (_agent.remainingDistance <= _agent.stoppingDistance)
                {
                    if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
                    {
                        if (hasResource) {
                            DeliverResource(destination, resource.ResourceType);
                        } else {
                            TakeResource(destination, resource.ResourceType);
                        }
                        hasDestination = false;
                    }
                }
            }
        }

        private void TakeResource(Building building, ResourceType resourceType)
        {
            hasResource = true;
            building.RemoveResource(resourceType, 1);
        }

        private void DeliverResource(Building building, ResourceType resourceType)
        {
            hasResource = false;
            building.AddResource(resourceType, 1);
        }

        private void PerformNextJob()
        {
            Job job = jobQueue.GetNextJob();
            if (job == null) {
                return;
            }
            destination = job.Destination;
            resource.ResourceType = job.ResourceType;
            _agent.SetDestination(destination.transform.position);
            hasDestination = true;
            return;
        }

        private void SetDestination()
        {
            if (hasResource)
            {
                List<Building> buildings = _buildingList.GetBuildings();
                destination = null; // Initially set destination to null
                Building warehouse = null;
                ResourceType resourceType = resource.ResourceType;

                foreach (var building in buildings)
                {
                    if (building.GetBuildingType() == BuildingType.Warehouse)
                    {
                        warehouse = building;
                        continue;
                    }
                    var inputTypes = building.GetInputTypes();
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
                    destination = building;
                    break;
                }

                // If no suitable building is found, set destination to Warehouse
                if (destination == null)
                {
                    destination = warehouse;
                    if (destination == null)
                    {
                        throw new Exception("No Destination found");
                    }
                }
                _agent.SetDestination(destination.transform.position);
                hasDestination = true;
            }
            else
            {
                PerformNextJob();
            }
        }
        
    }
}