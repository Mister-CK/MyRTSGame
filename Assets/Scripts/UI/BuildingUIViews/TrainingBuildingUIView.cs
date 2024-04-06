using System.Collections.Generic;
using MyRTSGame.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class TrainingBuildingUIView : MonoBehaviour
{
    [SerializeField] private Image trainingBuildingView;
    [SerializeField] private TextMeshProUGUI trainingBuildingName;
    [SerializeField] private GameObject inputTitlePrefab;
    [SerializeField] private GameObject columnsPrefab;
    [SerializeField] private GameObject resourceRowPrefab;
    [SerializeField] private GameObject inputLayoutGrid;
    
    [SerializeField] private GameEvent onNewVillagerEvent;

    private TrainingBuilding _currentTrainingBuilding;
    
    private List<ResourceRowOutput> _resourceRowOutputs = new List<ResourceRowOutput>();
    public void ActivateTrainingBuildingView(TrainingBuilding trainingBuilding)
    {
        _currentTrainingBuilding = trainingBuilding;
        trainingBuildingView.gameObject.SetActive(true);
        trainingBuildingName.text = trainingBuilding.BuildingType.ToString();
        Instantiate(inputTitlePrefab, inputLayoutGrid.transform);
        Instantiate(columnsPrefab, inputLayoutGrid.transform);

        foreach (var res in trainingBuilding.InventoryWhenCompleted)
        {
            var resourceRow = Instantiate(resourceRowPrefab, inputLayoutGrid.transform);
            var resourceRowOutput = resourceRow.GetComponent<ResourceRowOutput>();
            resourceRow.GetComponent<ResourceRowOutput>().ResourceType.text = res.ResourceType.ToString();
            resourceRow.GetComponent<ResourceRowOutput>().Quantity.text = res.Quantity.ToString();
            _resourceRowOutputs.Add(resourceRowOutput);
        }
    }
    
    public void UpdateResourceQuantities(TrainingBuilding trainingBuilding)
    {
        for (var i = 0; i < _resourceRowOutputs.Count; i++)
        {
            _resourceRowOutputs[i].UpdateQuantity(trainingBuilding.InventoryWhenCompleted[i].Quantity);
        }
    }
    
    public void DeactivateTrainingBuildingView()
    {
        // Destroy all child objects (rows) of the outputLayoutGrid
        foreach (Transform child in inputLayoutGrid.transform)
        {
            Destroy(child.gameObject);
        }
        _resourceRowOutputs = new List<ResourceRowOutput>();
        trainingBuildingView.gameObject.SetActive(false);
    }
    
    public void OnNewVillButtonClick()
    {
        Debug.Log("New villager button clicked" + _currentTrainingBuilding.transform.position);
        onNewVillagerEvent.Raise(new TrainingBuildingEventArgs(_currentTrainingBuilding));
    }
}
