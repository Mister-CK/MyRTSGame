using UnityEngine;
using UnityEngine.AI;

namespace MyRTSGame.Model
{
    public abstract class Unit: MonoBehaviour, ISelectable
    {
        protected NavMeshAgent Agent;
        protected SelectionManager SelectionManager;
        protected bool HasDestination;

        private void Awake()
        {
            Agent = GetComponentInChildren<NavMeshAgent>();
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
        
        protected void CheckIfDestinationIsReached()
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
            ExcecuteJob();
        }

        protected virtual void ExcecuteJob() {}
        protected virtual void SetDestination() {}

    }
}