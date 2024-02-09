using UnityEngine;
using UnityEngine.AI;

namespace MyRTSGame.Model
{
    public abstract class Unit: MonoBehaviour, ISelectable
    {
        protected NavMeshAgent Agent;
        protected SelectionManager SelectionManager;
        protected bool HasDestination;
        protected BuildingController BuildingController;

        private void Awake()
        {
            Agent = GetComponentInChildren<NavMeshAgent>();
        }

        protected virtual void Start()
        {
            BuildingController = BuildingController.Instance;
            SelectionManager = SelectionManager.Instance;
        }

        
        protected void Update()
        {
            if (HasDestination)
                CheckIfDestinationIsReached();
            else
                SetDestination();
        }
        
        public void HandleClick()
        {
            SelectionManager.SelectObject(this);
        }
        
        private void CheckIfDestinationIsReached()
        {
            if (Agent.pathPending)
            {
                return;
            }
            if (GetPathRemainingDistance(Agent) > Agent.stoppingDistance + 0.1f)
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
            
            // Destination reached
            ExecuteJob();
        }
        
        private static float GetPathRemainingDistance(NavMeshAgent navMeshAgent)
        {
            if (navMeshAgent.pathPending ||
                navMeshAgent.pathStatus == NavMeshPathStatus.PathInvalid ||
                navMeshAgent.path.corners.Length == 0)
                return -1f;

            var distance = 0.0f;
            for (var i = 0; i < navMeshAgent.path.corners.Length - 1; ++i)
            {
                distance += Vector3.Distance(navMeshAgent.path.corners[i], navMeshAgent.path.corners[i + 1]);
            }

            return distance;
        }

        protected virtual void ExecuteJob() {}
        protected virtual void SetDestination() {}
            
    }
}