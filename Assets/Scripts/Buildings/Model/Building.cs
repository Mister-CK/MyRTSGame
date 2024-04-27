using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MyRTSGame.Model
{
    public abstract class Building : MonoBehaviour, ISelectable, IDestination
    {
        [SerializeField] private GameEvent onSelectionEvent;

        public bool HasInput;
        private GameObject _buildingObject;
        public int Capacity = 999;
        public int capacityForCompletedBuilding { get; set; }
        public Dictionary<ResourceType, InventoryData> Inventory { get; set; }        
        public IBuildingState State;
        public Material Material { get; set; }
        protected BuildingType BuildingType;
        public ResourceType[] InputTypes { get; set; }
        public ResourceType[] InputTypesWhenCompleted { get; set; }
        public ResourceType[] OutputTypesWhenCompleted { get; set; }
        public BoxCollider BCollider { get; private set; }
        protected BuildingList BuildingList;
        protected UnitType OccupantType = UnitType.Villager;
        
        public int resourceCountNeededForConstruction = 0;
        public BuildingController buildingController;
        private List<VillagerJob> VillagerJobsToThisBuilding = new List<VillagerJob>();
        private List<VillagerJob> VillagerJobsFromThisBuilding = new List<VillagerJob>();
        private List<BuilderJob> builderJobsForThisBuilding = new List<BuilderJob>();
        private List<ConsumptionJob> consumptionJobsForThisbuilding = new List<ConsumptionJob>();

        private void Awake()
        {
            BCollider = this.AddComponent<BoxCollider>();
            BCollider.size = new Vector3(3, 3, 3);
            
            // I don't think this should be necessary
            InputTypes = new ResourceType[0];
            OutputTypesWhenCompleted = new ResourceType[0];
            InputTypesWhenCompleted = new ResourceType[0];

            Inventory = InitInventory(InputTypes);
            
            buildingController = BuildingController.Instance;
        }

        public Vector3 GetPosition()
        {
            return this.transform.position;
        }

        public void SetBuildingType(BuildingType buildingType)
        {
            BuildingType = buildingType;
        }
        
        public BuildingType GetBuildingType()
        {
            return BuildingType;
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
            State.SetObject(this);
            buildingController.CreateUpdateViewForBuildingEvent(this);

            if (State is CompletedState)
            {
                buildingController.CreateJobNeededEvent(JobType.LookForBuildingJob, this, null, null, OccupantType);
                StartResourceCreationCoroutine();
            }
            
            if (State is ConstructionState) buildingController.CreateJobNeededEvent(JobType.BuilderJob, this, null, null, null);
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
        
        public virtual void AddResource(ResourceType resourceType, int quantity)
        {
            Inventory[resourceType].Incoming -= quantity;
            Inventory[resourceType].Current += quantity;
            if (State is FoundationState foundationState) foundationState.CheckRequiredResources(this);
        }

        public void DeleteBuilding()
        {
            BuildingList.RemoveBuilding(this);
            buildingController.CreateDeleteJobsForBuildingEvent(VillagerJobsFromThisBuilding, VillagerJobsToThisBuilding, builderJobsForThisBuilding);
            Destroy(gameObject);
        }

        public void AddVillagerJobFromThisBuilding(Job job )
        {
            if (job is not VillagerJob villagerJob) return;
            Inventory[villagerJob.ResourceType].Outgoing++;
            VillagerJobsFromThisBuilding.Add(villagerJob);
        }
        
        public void AddJobToDestination(Job job)
        {
            switch (job)
            {
                case VillagerJob villagerJob:
                    Inventory[villagerJob.ResourceType].Incoming++;
                    VillagerJobsToThisBuilding.Add(villagerJob);
                    return;
                case BuilderJob builderJob:
                    builderJobsForThisBuilding.Add(builderJob);
                    return;
                case ConsumptionJob consumptionJob:
                    consumptionJobsForThisbuilding.Add(consumptionJob);
                    return;
            }
        }
        
        public void RemoveVillagerJobFromThisBuilding(VillagerJob villagerJob)
        {
            Inventory[villagerJob.ResourceType].Outgoing--;
            VillagerJobsFromThisBuilding.Remove(villagerJob);
        }
        
        public void RemoveJobFromDestination(Job job)
        {
            switch (job)
            {
                case VillagerJob villagerJob:
                    Inventory[villagerJob.ResourceType].Incoming--;
                    VillagerJobsToThisBuilding.Remove(villagerJob);
                    return;
                case BuilderJob builderJob:
                    builderJobsForThisBuilding.Remove(builderJob);
                    return;
                case ConsumptionJob consumptionJob:
                    consumptionJobsForThisbuilding.Remove(consumptionJob);
                    return;
            }
        }
    }
}