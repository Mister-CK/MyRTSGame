using Application;
using Application.Services;
using Buildings.Model.BuildingStates;
using Enums;
using Interface;
using MyRTSGame.Model;
using System;
using System.Collections.Generic;
using Units.Model.Component;
using Unity.VisualScripting;
using UnityEngine;

namespace Buildings.Model
{
    public abstract class Building : MonoBehaviour, ISelectable, IDestination, IInventory, IState<IBuildingState>, IBuildable
    {
        [SerializeField] private GameEvent onSelectionEvent;

        public bool HasInput;
        public int Capacity = 999;
        public int CapacityForCompletedBuilding { get; set; }
        public int resourceCountNeededForConstruction = 0;
        
        protected IBuildingState State;
        protected Dictionary<ResourceType, InventoryData> Inventory;    
        public Material Material { get; set; }
        public BoxCollider BCollider { get; private set; }
        private GameObject _buildingObject;
        protected BuildingType BuildingType;
        public ResourceType[] InputTypes { get; set; }
        public ResourceType[] InputTypesWhenCompleted { get; set; }
        public ResourceType[] OutputTypesWhenCompleted { get; set; }
        protected BuildingList BuildingList;
        protected UnitType OccupantType = UnitType.Villager;
        private UnitComponent _occupant;
        private readonly float _buildRate = 1f;
        
        private readonly List<VillagerJob> _villagerJobsToThisBuilding = new List<VillagerJob>();
        private readonly List<VillagerJob> _villagerJobsFromThisBuilding = new List<VillagerJob>();
        private readonly List<BuilderJob> _builderJobsForThisBuilding = new List<BuilderJob>();
        private readonly List<ConsumptionJob> _consumptionJobsForThisbuilding = new List<ConsumptionJob>();
        private readonly List<LookingForBuildingJob> _lookingForBuildingJobsForThisBuilding = new List<LookingForBuildingJob>();

        public BuildingService BuildingService { get; set; }
        private void Awake()
        {
            ServiceInjector.Instance.InjectBuildingDependenies(this);
            BCollider = this.AddComponent<BoxCollider>();
            BCollider.size = new Vector3(3, 3, 3);
            
            // I don't think this should be necessary
            InputTypes = new ResourceType[0];
            OutputTypesWhenCompleted = new ResourceType[0];
            InputTypesWhenCompleted = new ResourceType[0];

            Inventory = InventoryHelper.InitInventory(InputTypes);
            
        }

        protected virtual void Start()
        {
            BuildingList = BuildingList.Instance; 
            State = new PlacingState(BuildingType);
            
            CapacityForCompletedBuilding = 5;
            resourceCountNeededForConstruction = 3;
        }

        private void Update()
        {
            if (State is PlacingState placingState) PlacingState.CheckOverlap(this);
        }
        
        public void SetOccupant(UnitComponent unit)
        {
            _occupant = unit;
        }
        
        public UnitComponent GetOccupant()
        {
            return _occupant;
        }
        
        public UnitType GetOccupantType()
        {
            return OccupantType;
        }
        
        public void SetOccupantType(UnitType unitType)
        {
            OccupantType = unitType;
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public void SetBuildingType(BuildingType buildingType)
        {
            BuildingType = buildingType;
        }
        
        public BuildingType GetBuildingType()
        {
            return BuildingType;
        }
        
        public void OnMouseDown()
        {
            if (State is PlacingState) return;
            onSelectionEvent.Raise(new SelectionEventArgs(this));
        }

        protected virtual void StartResourceCreationCoroutine()
        {
            // This method can be overridden in derived classes to start the specific coroutine for each building type.
        }
        
        public void SetState(IBuildingState newState)
        {
            State = newState;
            State.SetObject(this);
            BuildingService.CreateUpdateViewForBuildingEvent(this);

            if (State is CompletedState)
            {
                BuildingService.CreateJobNeededEvent(JobType.LookForBuildingJob, this, null, null, OccupantType);
                StartResourceCreationCoroutine();
            }
            
            if (State is ConstructionState) BuildingService.CreateJobNeededEvent(JobType.BuilderJob, this, null, null, null);
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

        public void SetInventory(Dictionary<ResourceType, InventoryData>  inventory)
        {
            Inventory = inventory;
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
            Inventory[resourceType].Outgoing -= quantity;
            Inventory[resourceType].Current -= quantity;
        }
        
        public virtual void AddResource(ResourceType resourceType, int quantity)
        {
            Inventory[resourceType].Incoming -= quantity;
            Inventory[resourceType].Current += quantity;
            if (State is FoundationState foundationState) foundationState.CheckRequiredResources(this);
        }

        public void DeleteBuilding()
        {
            BuildingList.RemoveBuilding(this);
            if (_occupant != null) BuildingService.CreateDeleteBuildingForOccupantEvent(this);
            BuildingService.CreateDeleteJobsForBuildingEvent(_villagerJobsFromThisBuilding, _villagerJobsToThisBuilding, _builderJobsForThisBuilding);
            Destroy(gameObject);
        }

        public void AddVillagerJobFromThisBuilding(Job job )
        {
            if (job is not VillagerJob villagerJob) return;
            Inventory[villagerJob.ResourceType].InJob++;
            _villagerJobsFromThisBuilding.Add(villagerJob);
        }
        
        public void AddJobToDestination(Job job)
        {
            switch (job)
            {
                case VillagerJob villagerJob:
                    Inventory[villagerJob.ResourceType].InJob++;
                    _villagerJobsToThisBuilding.Add(villagerJob);
                    return;
                case BuilderJob builderJob:
                    _builderJobsForThisBuilding.Add(builderJob);
                    return;
                case ConsumptionJob consumptionJob:
                    _consumptionJobsForThisbuilding.Add(consumptionJob);
                    return;
                case LookingForBuildingJob lookingForBuildingJob:
                    _lookingForBuildingJobsForThisBuilding.Add(lookingForBuildingJob);
                    return;
                default: throw new System.ArgumentException("job type not recognized in AddJobToDestination");
            }
        }
        
        public void RemoveVillagerJobFromThisBuilding(VillagerJob villagerJob)
        {
            Inventory[villagerJob.ResourceType].InJob--;
            _villagerJobsFromThisBuilding.Remove(villagerJob);
        }
        
        public void RemoveJobFromDestination(Job job)
        {
            switch (job)
            {
                case VillagerJob villagerJob:
                    Inventory[villagerJob.ResourceType].InJob--;
                    _villagerJobsToThisBuilding.Remove(villagerJob);
                    return;
                case BuilderJob builderJob:
                    _builderJobsForThisBuilding.Remove(builderJob);
                    return;
                case ConsumptionJob consumptionJob:
                    _consumptionJobsForThisbuilding.Remove(consumptionJob);
                    return;
            }
        }
        
        public void ModifyInventory(ResourceType resourceType, Action<InventoryData> modifyAction)
        {
            if (Inventory.TryGetValue(resourceType, out var inventoryData))
            {
                modifyAction(inventoryData);
            }
            else
            {
                throw new ArgumentException($"The inventory does not contain the resource type: {resourceType}");
            }
        }
        
        public float GetBuildRate()
        {
            return _buildRate;
        }
    }
    
}