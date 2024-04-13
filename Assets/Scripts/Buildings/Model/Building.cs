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
        public Dictionary<ResourceType, int> Inventory { get; set; }        
        public Dictionary<ResourceType, int> InventoryWhenCompleted { get; set; }        

        public Resource[] ResourcesInJobForBuilding { get; set; }

        private Resource[] IncomingResources { get; set; }
        private Resource[] OutgoingResources { get; set; }

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
            var resourceQuantities = new int[0];
            InputTypes = new ResourceType[0];
            Inventory = InitInventory(resourceTypes);
            ResourcesInJobForBuilding = InitInventory(resourceTypes, resourceQuantities);
            IncomingResources = InitInventory(resourceTypes, resourceQuantities);
            OutgoingResources = InitInventory(resourceTypes, resourceQuantities);

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
        
        public static Dictionary<ResourceType, int> InitInventory(ResourceType[] resTypes)
        {
            var inventory = new Dictionary<ResourceType, int>();

            foreach (var resType in resTypes)
            {
                inventory[resType] = 0;
            }

            return inventory;
        }

        public Dictionary<ResourceType, int> GetInventory()
        {
            return Inventory;
        }

        public int GetCapacity()
        {
            return Capacity;
        }
        
        public void RemoveResource(ResourceType resourceType, int quantity)
        {
            Inventory[resourceType] -= quantity;
        }
        
        public void AddResource(ResourceType resourceType, int quantity)
        {
            foreach (var resource in ResourcesInJobForBuilding)
            {
                if (resource.ResourceType != resourceType) continue;

                resource.Quantity -= quantity;
                break;
            }
            
            foreach (var resource in IncomingResources)
            {
                if (resource.ResourceType != resourceType) continue;

                resource.Quantity -= quantity;
                break;
            }

            Inventory[resourceType] += quantity;
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
            foreach (var res in IncomingResources)
            {
                if (res.ResourceType == job.ResourceType)
                {
                    res.Quantity++;
                    break;
                }
            }
            VillagerJobsToThisBuilding.Add(job);
        }

        public void AddVillagerJobFromThisBuilding(VillagerJob job )
        {
            foreach (var res in OutgoingResources)
            {
                if (res.ResourceType == job.ResourceType)
                {
                    res.Quantity++;
                    break;
                }
            }
            VillagerJobsFromThisBuilding.Add(job);
        }
        
        public Resource[] GetOutgoingResources()
        {
            return OutgoingResources;
        }
        
        public Resource[] GetIncomingResources()
        {
            return IncomingResources;
        }
        
        public void SetOutgoingResources(Resource[] outgoingResources)
        {
            OutgoingResources = outgoingResources;
        }
        
        public void SetIncomingResources(Resource[] incomingResources)
        {
            IncomingResources = incomingResources;
        }

    }
}