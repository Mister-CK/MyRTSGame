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

        protected virtual void Start()
        {
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
            ExecuteJob();
        }

        protected virtual void ExecuteJob() {}
        protected virtual void SetDestination() {}
            
    }
}