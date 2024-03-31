using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyRTSGame.Model
{
    public class BuildingController : MonoBehaviour
    {

        [SerializeField] private GameEvent onResourceRemovedFromBuilding;
        [SerializeField] private GameEvent onResourceAddedToBuilding;
        [SerializeField] private GameEvent onNewVillagerJobNeeded;
        [SerializeField] private GameEvent onNewBuilderJobNeeded;

        public static BuildingController Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }
        
        private void OnEnable()
        {
            onResourceAddedToBuilding.RegisterListener(OnResourceAdded);
            onResourceRemovedFromBuilding.RegisterListener(OnResourceRemoved);

        }

        private void OnDisable()
        {
            onResourceAddedToBuilding.UnregisterListener(OnResourceAdded);
            onResourceRemovedFromBuilding.UnregisterListener(OnResourceRemoved);
            
        }
        
        private static void OnResourceAdded(IGameEventArgs args)
        {
            if (args is BuildingResourceTypeQuantityEventArgs eventArgs)
            {
                eventArgs.Building.AddResource(eventArgs.ResourceType, eventArgs.Quantity);
            }
        }

        private static void OnResourceRemoved(IGameEventArgs args)
        {
            if (args is BuildingResourceTypeQuantityEventArgs eventArgs)
            {
                eventArgs.Building.RemoveResource(eventArgs.ResourceType, eventArgs.Quantity);
            }
        }
        
        public void CreateVillagerJobNeededEvent(Building building, ResourceType resourceType)
        {
            onNewVillagerJobNeeded.Raise(new BuildingResourceTypeEventArgs(building, resourceType));
        }
        
        public void CreateNewBuilderJobNeededEvent(Building building)
        {
            onNewBuilderJobNeeded.Raise(new BuildingEventArgs(building));
        }
    }
}