using System.Collections.Generic;
using MyRTSGame.Model;
using MyRTSGame.Model.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class TrainingBuildingUIView : MonoBehaviour
{
    [SerializeField] private Image trainingBuildingView;
    [SerializeField] private TextMeshProUGUI trainingBuildingName;
    
    [SerializeField] private GameObject columnsPrefab;
    [SerializeField] private GameObject resourceRowPrefab;
    [SerializeField] private GameObject resourceRowTrainingPrefab;

    [SerializeField] private GameObject inputTitlePrefab;
    [SerializeField] private GameObject inputLayoutGrid;
    
    [SerializeField] private GameObject jobQueueLayoutGrid;
    [SerializeField] private GameObject jobQueueTitlePrefab;
    
    private TrainingBuilding _currentTrainingBuilding;
    
    private List<ResourceRowOutput> _resourceRows = new List<ResourceRowOutput>();
    private List<ResourceRowTraining> _jobRows = new List<ResourceRowTraining>();
    
    Dictionary<ResourceType, int> resourceQuantities = new Dictionary<ResourceType, int>();

    
    public void ActivateTrainingBuildingView(TrainingBuilding trainingBuilding)
    {
        _currentTrainingBuilding = trainingBuilding;
        trainingBuildingView.gameObject.SetActive(true);
        trainingBuildingName.text = trainingBuilding.BuildingType.ToString();
        Instantiate(inputTitlePrefab, inputLayoutGrid.transform);
        Instantiate(columnsPrefab, inputLayoutGrid.transform);
        
        Instantiate(jobQueueTitlePrefab, jobQueueLayoutGrid.transform);
        Instantiate(columnsPrefab, jobQueueLayoutGrid.transform);
        
        foreach (var res in trainingBuilding.InventoryWhenCompleted)
        {
            resourceQuantities[res.ResourceType] = res.Quantity;
        }
        
        foreach (var inputType in trainingBuilding.InputTypesWhenCompleted)
        {
            var resourceRow = Instantiate(resourceRowPrefab, inputLayoutGrid.transform);
            var resourceRowInput = resourceRow.GetComponent<ResourceRowOutput>();
            resourceRowInput.resourceTypeText.text = inputType.ToString();
            resourceRowInput.quantity.text = resourceQuantities[inputType].ToString();
            _resourceRows.Add(resourceRowInput);
        }

        foreach (var trainingJob in trainingBuilding.TrainingJobs)
        {
            var resourceRow = Instantiate(resourceRowTrainingPrefab, jobQueueLayoutGrid.transform);
            var resourceRowJobQueue = resourceRow.GetComponent<ResourceRowTraining>();
            resourceRowJobQueue.UnitType = trainingJob.UnitType;
            resourceRowJobQueue.TrainingBuilding = trainingBuilding;
            resourceRowJobQueue.unitTypeText.text = trainingJob.UnitType.ToString();
            resourceRowJobQueue.quantity.text = trainingJob.Quantity.ToString();
            _jobRows.Add(resourceRowJobQueue);
        }
    }
    
    public void UpdateResourceQuantities(TrainingBuilding trainingBuilding)
    {
        for (var i = 0; i < _resourceRows.Count; i++)
        {
            _resourceRows[i].UpdateQuantity(trainingBuilding.Inventory[i].Quantity);
        }
        
        var jobRowsCount = _jobRows.Count;
        for (var i = 0; i < jobRowsCount; i++)
        {
            var unitType = _jobRows[i].UnitType;
            _jobRows[i].UpdateQuantity(trainingBuilding.TrainingJobs.Find(el => el.UnitType == unitType).Quantity);
        }
    }
    
    public void DeactivateTrainingBuildingView()
    {
        // Destroy all child objects (rows) of the outputLayoutGrid
        foreach (Transform child in inputLayoutGrid.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in jobQueueLayoutGrid.transform)
        {
            Destroy(child.gameObject);
        }
        _resourceRows = new List<ResourceRowOutput>();
        _jobRows = new List<ResourceRowTraining>();
        trainingBuildingView.gameObject.SetActive(false);
    }
}
