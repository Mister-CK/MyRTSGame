using System.Collections.Generic;
using MyRTSGame.Model;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class FoundationStateBuildingUIView : MonoBehaviour
{
    [SerializeField] private Image foundationStateBuildingView;
    [SerializeField] private TextMeshProUGUI foundationStateBuildingName;
    [SerializeField] private GameObject inputTitlePrefab;
    [SerializeField] private GameObject columnsPrefab;
    [SerializeField] private GameObject resourceRowOutputPrefab;
    [SerializeField] private GameObject inputLayoutGrid;
    
    private List<ResourceRowOutput> _resourceRowOutputs = new List<ResourceRowOutput>();
    public void ActivateFoundationStateBuildingView(Building building)
    {
        foundationStateBuildingView.gameObject.SetActive(true);
        foundationStateBuildingName.text = building.BuildingType.ToString();
        Instantiate(inputTitlePrefab, inputLayoutGrid.transform);
        Instantiate(columnsPrefab, inputLayoutGrid.transform);

        foreach (var res in building.Inventory)
        {
            var resourceRow = Instantiate(resourceRowOutputPrefab, inputLayoutGrid.transform);
            var resourceRowOutput = resourceRow.GetComponent<ResourceRowOutput>();
            resourceRow.GetComponent<ResourceRowOutput>().ResourceType.text = res.ResourceType.ToString();
            resourceRow.GetComponent<ResourceRowOutput>().Quantity.text = res.Quantity.ToString();
            _resourceRowOutputs.Add(resourceRowOutput);
        }
    }
    
    public void UpdateResourceQuantities(Building building)
    {
        for (int i = 0; i < _resourceRowOutputs.Count; i++)
        {
            _resourceRowOutputs[i].UpdateQuantity(building.Inventory[i].Quantity);
        }
    }
    
    public void DeactivateFoundationStateBuildingView()
    {
        // Destroy all child objects (rows) of the outputLayoutGrid
        foreach (Transform child in inputLayoutGrid.transform)
        {
            Destroy(child.gameObject);
        }
        _resourceRowOutputs = new List<ResourceRowOutput>();
        foundationStateBuildingView.gameObject.SetActive(false);
    }
}
