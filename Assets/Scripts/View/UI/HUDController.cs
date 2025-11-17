using Buildings.Model;
using Buildings.Model.BuildingGroups;
using Enums;
using Interface;
using Terrains;
using Domain.Units.Component;
using UnityEngine;
using UnityEngine.UIElements;

namespace View
{

    public class HUDController : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;
        [SerializeField] private StyleSheet stylesheet;
        
        [SerializeField] private GameEvent onDeleteBuildingEvent;
        [SerializeField] private GameEvent onDeleteUnitEvent;
        [SerializeField] private GameEvent onRemoveOccupantFromBuildingEvent;

        [SerializeField] private GameEvent onAddProductionJobEvent;
        [SerializeField] private GameEvent onRemoveProductionJobEvent;
        
        [SerializeField] private GameEvent onAddTrainingJobEvent;
        [SerializeField] private GameEvent onRemoveTrainingJobEvent;
        
        [SerializeField] private GameEvent onSelectionEvent;
        [SerializeField] private GameEvent onDeselectionEvent;

        private HUDView _hudView;

        private void OnEnable()
        {
            onSelectionEvent.RegisterListener(HandleSelectObject);
            onDeselectionEvent.RegisterListener(HandleDeselectObject);
        }

        private void OnDisable()
        {
            onSelectionEvent.UnregisterListener(HandleSelectObject);
            onDeselectionEvent.RegisterListener(HandleDeselectObject);

        }
        
        private void Awake()
        {
            var buildingPlacer = FindFirstObjectByType<BuildingPlacer>();
            var terrainPlacer = FindFirstObjectByType<TerrainPlacer>();
            
            _hudView  = new HUDView(this, buildingPlacer, terrainPlacer);
        }

        private void Start()
        {
            _hudView.Initialize(uiDocument, stylesheet);
        }
        
        private void Update()
        {
            _hudView.UpdateSelectionPanel();
        }
        
        private void LateUpdate()
        {
            if (Input.GetMouseButtonDown(1)) _hudView.HideAllPanels();
        }
        private void HandleDeselectObject(IGameEventArgs args)
        {
            _hudView.HideAllPanels();
        }
        
        public void ShowSelectionPanel(ISelectable selectedObject)
        {
            _hudView.ShowSelectionPanel(selectedObject);
        }
        
        private void HandleSelectObject(IGameEventArgs args)
        {
            if (args is not SelectionEventArgs selectionEventArgs) return;
            _hudView.ShowSelectionPanel(selectionEventArgs.SelectedObject);
        }
        
        public void CreateDeleteEvent(ISelectable selectedObject)
        {
            if (selectedObject is Building selectedBuilding)
            {
                onDeleteBuildingEvent.Raise(new BuildingEventArgs(selectedBuilding));
            }
            
            if (selectedObject is UnitComponent selectedUnit)
            {
                if (selectedUnit is ResourceCollectorComponent resourceCollector)
                {
                    var building = resourceCollector.CollectorData.Building;
                    if (building != null)
                    {
                        onRemoveOccupantFromBuildingEvent.Raise(new BuildingEventArgs(building));
                    }
                }
                onDeleteUnitEvent.Raise(new UnitEventArgs(selectedUnit));
            }
            _hudView.ShowSelectionPanel(null);
        }
        public void ReduceProduction(WorkshopBuilding workshopBuildingbuilding, ResourceType resourceType)
        {
            onRemoveProductionJobEvent.Raise(new WorkshopBuildingBuildingResourceTypeEventArgs(workshopBuildingbuilding, resourceType));
        }

        public void IncreaseProduction(WorkshopBuilding workshopBuildingbuilding, ResourceType resourceType)
        {
            onAddProductionJobEvent.Raise(new WorkshopBuildingBuildingResourceTypeEventArgs(workshopBuildingbuilding, resourceType));
        }
        
        public void RemoveTrainingJob(TrainingBuilding trainingBuilding, UnitType unitType)
        {
            onRemoveTrainingJobEvent.Raise(new TrainingBuildingUnitTypeEventArgs(trainingBuilding, unitType));
        }

        public void AddTrainigJob(TrainingBuilding trainingBuilding, UnitType unitType)
        {
            onAddTrainingJobEvent.Raise(new TrainingBuildingUnitTypeEventArgs(trainingBuilding, unitType));
        }
    }
}