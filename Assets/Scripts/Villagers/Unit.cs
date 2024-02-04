using UnityEngine;
using UnityEngine.AI;

namespace MyRTSGame.Model
{
    public abstract class Unit: MonoBehaviour, ISelectable
    {
        protected NavMeshAgent Agent;
        protected SelectionManager SelectionManager;

        private void Awake()
        {
            Agent = GetComponentInChildren<NavMeshAgent>();
        }
        
        public void HandleClick()
        {
            SelectionManager.SelectObject(this);
        }
    }
}