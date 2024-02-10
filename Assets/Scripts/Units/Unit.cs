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
        protected Building Destination;
        [SerializeField] private GameEvent onSelectionEvent;
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
            onSelectionEvent.Raise(new UnitEventArgs(this));
        }
        
        private Vector3 _lastPosition;
        private float _stuckTimer;

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