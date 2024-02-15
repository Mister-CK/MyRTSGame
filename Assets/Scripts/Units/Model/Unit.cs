using UnityEngine;
using UnityEngine.AI;

namespace MyRTSGame.Model
{
    public abstract class Unit: MonoBehaviour, ISelectable
    {
        protected NavMeshAgent Agent;
        protected bool HasDestination;
        protected Building Destination;
        [SerializeField] protected UnitController unitController;
        
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
        


        private void CheckIfDestinationIsReached()
        {
            if (!Agent.pathPending)
            {
                if (Agent.remainingDistance <= Agent.stoppingDistance && Agent.pathStatus == NavMeshPathStatus.PathComplete)
                {
                    if (!Agent.hasPath || Agent.velocity.sqrMagnitude == 0f)
                    {
                        // The agent has reached its destination
                        ExecuteJob();
                    }
                }
            }
        }

        protected virtual void ExecuteJob() {}
        protected virtual void SetDestination() {}
            
    }
}