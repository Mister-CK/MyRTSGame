using Buildings.Model;
using Buildings.Model.BuildingGroups;
using Enums;
using Interface;
using System.Collections.Generic;
using UI.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.BuildingUIViews
{
    public class CompletedStateBuildingUIView : MonoBehaviour
    {
        [SerializeField] private Image buildingView;
        [SerializeField] private TextMeshProUGUI buildingName;
        [SerializeField] private GameObject columnsPrefab;
        [SerializeField] private GameObject occupantButton;

        [SerializeField] private GameObject inputLayoutGrid;
        [SerializeField] private GameObject inputTitlePrefab;
        [SerializeField] private GameObject resourceRowInputPrefab;

        [SerializeField] private GameObject outputLayoutGrid;
        [SerializeField] private GameObject outputTitlePrefab;
        [SerializeField] private GameObject resourceRowOutputPrefab;

        [SerializeField] private GameObject trainingJobLayoutGrid;
        [SerializeField] private GameObject trainingJobTitlePrefab;
        [SerializeField] private GameObject resourceRowTrainingPrefab;

        [SerializeField] private GameObject jobQueueLayoutGrid;
        [SerializeField] private GameObject jobQueueTitlePrefab;
        [SerializeField] private GameObject resourceRowProductionPrefab;

        private readonly Dictionary<GameObject, List<GameObject>> _rowPool = new();
        private readonly List<GameObject> _activeChildren = new();

        private readonly Dictionary<ResourceType, int> _resourceQuantities = new Dictionary<ResourceType, int>();

        private List<IResourceRow> _resourceRowsInput = new();
        private List<IResourceRow> _resourceRowsOutput = new();
        private List<ResourceRowProduction> _resourceRowProduction = new();
        private List<ResourceRowTraining> _resourceRowsTraining = new();

        public void SetOccupantButton(Building building)
        {
            occupantButton.SetActive(building.GetOccupant() != null);
        }

        public void ActivateBuildingView(Building building)
        {
            buildingView.gameObject.SetActive(true);
            buildingName.text = building.GetBuildingType().ToString();
            SetOccupantButton(building);

            foreach (var res in building.GetInventory())
            {
                _resourceQuantities[res.Key] = res.Value.Current;
            }

            if (building.InputTypesWhenCompleted.Length > 0)
            {
                GetOrCreatePooledObject(inputTitlePrefab, inputLayoutGrid.transform);
                GetOrCreatePooledObject(columnsPrefab, inputLayoutGrid.transform);
                foreach (var inputType in building.InputTypesWhenCompleted)
                {
                    var resourceRow = GetOrCreatePooledObject(resourceRowInputPrefab, inputLayoutGrid.transform);
                    var resourceRowInput = resourceRow.GetComponent<ResourceRowInput>();
                    resourceRowInput.ResourceType = inputType;
                    resourceRowInput.resourceTypeText.text = inputType.ToString();
                    resourceRowInput.quantity.text = _resourceQuantities[inputType].ToString();
                    _resourceRowsInput.Add(resourceRowInput);
                }
            }

            if (building.OutputTypesWhenCompleted.Length > 0)
            {
                GetOrCreatePooledObject(outputTitlePrefab, outputLayoutGrid.transform);
                GetOrCreatePooledObject(columnsPrefab, outputLayoutGrid.transform);
                foreach (var outputType in building.OutputTypesWhenCompleted)
                {
                    var resourceRow = GetOrCreatePooledObject(resourceRowOutputPrefab, outputLayoutGrid.transform);
                    var resourceRowOutput = resourceRow.GetComponent<ResourceRowOutput>();
                    resourceRowOutput.ResourceType = outputType;
                    resourceRowOutput.resourceTypeText.text = outputType.ToString();
                    resourceRowOutput.quantity.text = _resourceQuantities[outputType].ToString();
                    _resourceRowsOutput.Add(resourceRowOutput);
                }
            }

            if (building is TrainingBuilding trainingBuilding)
            {
                GetOrCreatePooledObject(trainingJobTitlePrefab, trainingJobLayoutGrid.transform);
                GetOrCreatePooledObject(columnsPrefab, trainingJobLayoutGrid.transform);
                foreach (var trainingJob in trainingBuilding.TrainingJobs)
                {
                    var resourceRow = GetOrCreatePooledObject(resourceRowTrainingPrefab, trainingJobLayoutGrid.transform);
                    var resourceRowJobQueue = resourceRow.GetComponent<ResourceRowTraining>();
                    resourceRowJobQueue.UnitType = trainingJob.UnitType;
                    resourceRowJobQueue.TrainingBuilding = trainingBuilding;
                    resourceRowJobQueue.unitTypeText.text = trainingJob.UnitType.ToString();
                    resourceRowJobQueue.quantity.text = trainingJob.Quantity.ToString();
                    _resourceRowsTraining.Add(resourceRowJobQueue);
                }
            }

            if (building is WorkshopBuilding workshopBuilding)
            {
                GetOrCreatePooledObject(jobQueueTitlePrefab, jobQueueLayoutGrid.transform);
                GetOrCreatePooledObject(columnsPrefab, jobQueueLayoutGrid.transform);
                foreach (var outputType in building.OutputTypesWhenCompleted)
                {
                    var resourceRow = GetOrCreatePooledObject(resourceRowProductionPrefab, jobQueueLayoutGrid.transform);
                    var resourceRowJobQueue = resourceRow.GetComponent<ResourceRowProduction>();
                    resourceRowJobQueue.ResourceType = outputType;
                    resourceRowJobQueue.WorkshopBuilding = workshopBuilding;
                    resourceRowJobQueue.resourceTypeText.text = outputType.ToString();
                    resourceRowJobQueue.quantity.text = _resourceQuantities[outputType].ToString();
                    _resourceRowProduction.Add(resourceRowJobQueue);
                }
            }
        }

        public void DeactivateBuildingView()
        {
            buildingView.gameObject.SetActive(false);

            foreach (var child in _activeChildren)
            {
                child.SetActive(false);
                child.transform.SetParent(this.transform); 
            }
            
            _activeChildren.Clear();

            _resourceRowProduction.Clear();
            _resourceRowsInput.Clear();
            _resourceRowsOutput.Clear();
            _resourceRowsTraining.Clear();
        }

        public void UpdateResourceQuantities(Building building)
        {
            UpdateResourceQuantities(_resourceRowsOutput, building);
            UpdateResourceQuantities(_resourceRowsInput, building);
            
            if (building is TrainingBuilding trainingBuilding)
            {
                foreach (var jobRow in _resourceRowsTraining)
                {
                    jobRow.UpdateQuantity(trainingBuilding.TrainingJobs.Find(el => el.UnitType == jobRow.UnitType).Quantity);
                }
            }

            if (building is WorkshopBuilding workshopBuilding)
            {
                foreach (var productionRow in _resourceRowProduction)
                {
                    productionRow.UpdateQuantity(workshopBuilding.ProductionJobs.Find(el => el.Output.ResourceType == productionRow.ResourceType).Quantity);
                }
            }
        }
        private void UpdateResourceQuantities(List<IResourceRow> resourceRows,  Building building)
        {
            var buildingInventory = building.GetInventory();
            foreach (var row in resourceRows)
            {
                var resType = row.ResourceType;
                
                if (!buildingInventory.TryGetValue(resType, out var resValue)) continue;

                row.UpdateQuantity(resValue.Current);
                if (row is ResourceRowOutput) row.UpdateJobs(resValue.Outgoing);
                if (row is ResourceRowInput) row.UpdateJobs(resValue.Incoming);
            }
        }
        
        private GameObject GetOrCreatePooledObject(GameObject prefab, Transform parent)
        {
            if (!_rowPool.ContainsKey(prefab))
            {
                _rowPool[prefab] = new List<GameObject>();
            }

            var pool = _rowPool[prefab];
    
            for (int i = 0; i < pool.Count; i++)
            {
                if (pool[i].activeSelf) continue;
                {
                    pool[i].SetActive(true);
                    pool[i].transform.SetParent(parent, false); 
                    _activeChildren.Add(pool[i]);
                    return pool[i];
                }
            }

            var newObject = Instantiate(prefab, parent);
            pool.Add(newObject);
            _activeChildren.Add(newObject);
            return newObject;
        }
    }
}