using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MyRTSGame.Model
{
    public abstract class Building : MonoBehaviour, ISelectable
    {
        [SerializeField] private GameEvent onSelectionEvent;

        public bool HasInput;
        private GameObject _buildingObject;
        public int Capacity = 999;
        public int capacityForCompletedBuilding { get; set; }
        public Dictionary<ResourceType, InventoryData> Inventory { get; set; }        
        public Dictionary<ResourceType, InventoryData> InventoryWhenCompleted { get; set; }        
        
        public IBuildingState State;
        public Material Material { get; set; }
        public BuildingType BuildingType { get; set; }
        public ResourceType[] InputTypes { get; set; }
        public ResourceType[] InputTypesWhenCompleted { get; set; }
        public ResourceType[] OutputTypesWhenCompleted { get; set; }
        public BoxCollider BCollider { get; private set; }
        protected BuildingList BuildingList;
        
        public int resourceCountNeededForConstruction = 0;
        public BuildingController buildingController;
        private List<VillagerJob> VillagerJobsToThisBuilding = new List<VillagerJob>();
        private List<VillagerJob> VillagerJobsFromThisBuilding = new List<VillagerJob>();
        
        private void Awake()
        {
            BCollider = this.AddComponent<BoxCollider>();
            BCollider.size = new Vector3(3, 3, 3);
            
            // I don't think this should be necessary
            var resourceTypes = new ResourceType[0];
            InputTypes = new ResourceType[0];
            Inventory = InitInventory(resourceTypes);
            
            buildingController = BuildingController.Instance;
        }

        protected virtual void Start()
        {
            
            BuildingList = BuildingList.Instance; 
            State = new PlacingState(BuildingType);
            
            capacityForCompletedBuilding = 5;
            resourceCountNeededForConstruction = 3;
        }

        private void Update()
        {
            if (State is PlacingState placingState) placingState.CheckOverlap(this);
        }

        public void OnMouseDown()
        {
            onSelectionEvent.Raise(new SelectionEventArgs(this));
        }

        public virtual void StartResourceCreationCoroutine()
        {
            // This method can be overridden in derived classes to start the specific coroutine for each building type.
        }
        
        public void SetState(IBuildingState newState)
        {
            State = newState;
            buildingController.CreateUpdateViewForBuildingEvent(this);
            // if (_building.State is ConstructionState) _building.State = new CompletedState(_building.BuildingType); // skip constructionState
            State.SetObject(this);

            if (State is CompletedState) StartResourceCreationCoroutine();
            
            if (newState is ConstructionState)
            {
                buildingController.CreateNewBuilderJobNeededEvent(this);
            }
        }
        
        public IBuildingState GetState()
        {
            return State;
        }

        public void SetObject(GameObject newObject)
        {
            if (_buildingObject != null) Destroy(_buildingObject);
            _buildingObject = Instantiate(newObject, transform.position + newObject.transform.position,
                Quaternion.identity, transform);
        }

        public static Resource[] InitInventory(ResourceType[] types, int[] quantities)
        {
            if (types == null || quantities == null)
            {
                return new Resource[0];
            }
                
            if (types.Length != quantities.Length)
                throw new ArgumentException("Types and quantities arrays must have the same length.");

            var resources = new Resource[types.Length];

            for (var i = 0; i < types.Length; i++)
                resources[i] = new Resource
                {
                    ResourceType = types[i],
                    Quantity = quantities[i]
                };

            return resources;
        }
        
        public static Dictionary<ResourceType, InventoryData> InitInventory(IEnumerable<ResourceType> resTypes)
        {
            var inventory = new Dictionary<ResourceType, InventoryData>();

            foreach (var resType in resTypes)
            {
                inventory[resType] = new InventoryData() { Incoming = 0, Current = 0, Outgoing = 0 };
            }

            return inventory;
        }

        public Dictionary<ResourceType, InventoryData> GetInventory()
        {
            return Inventory;
        }

        public int GetCapacity()
        {
            return Capacity;
        }
        
        public void RemoveResource(ResourceType resourceType, int quantity)
        {
            Inventory[resourceType].Current -= quantity;
        }
        
        public void AddResource(ResourceType resourceType, int quantity)
        {
            Inventory[resourceType].Incoming -= quantity;
            Inventory[resourceType].Current += quantity;
            if (State is FoundationState foundationState) foundationState.CheckRequiredResources(this);

        }

        public void DeleteBuilding()
        {
            BuildingList.RemoveBuilding(this);
            buildingController.CreateDeleteJobsForBuildingEvent(VillagerJobsFromThisBuilding, VillagerJobsToThisBuilding);
            Destroy(this);
        }
        
        public void AddVillagerJobToThisBuilding(VillagerJob job )
        {
            Inventory[job.ResourceType].Incoming++;
            VillagerJobsToThisBuilding.Add(job);
        }

        public void AddVillagerJobFromThisBuilding(VillagerJob job )
        {
            
            Inventory[job.ResourceType].Outgoing++;
            VillagerJobsFromThisBuilding.Add(job);
        }
    }
}