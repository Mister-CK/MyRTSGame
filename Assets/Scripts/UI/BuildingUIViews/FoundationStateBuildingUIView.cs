using Buildings.Model;
using Enums;
using System.Collections.Generic;
using System.Linq;
using MyRTSGame.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class FoundationStateBuildingUIView : MonoBehaviour
{
    [SerializeField] private Image foundationStateBuildingView;
    [SerializeField] private TextMeshProUGUI foundationStateBuildingName;
    [SerializeField] private GameObject inputTitlePrefab;
    [SerializeField] private GameObject columnsPrefab;
    [SerializeField] private GameObject resourceRowInputPrefab;
    [SerializeField] private GameObject inputLayoutGrid;
    
    private List<ResourceRowInput> _resourceRowsInput = new List<ResourceRowInput>();
    private Dictionary<ResourceType, int> resourceQuantities = new Dictionary<ResourceType, int>();

    public void ActivateFoundationStateBuildingView(Building building)
    {
        foundationStateBuildingView.gameObject.SetActive(true);
        foundationStateBuildingName.text = building.GetBuildingType().ToString();
        Instantiate(inputTitlePrefab, inputLayoutGrid.transform);
        Instantiate(columnsPrefab, inputLayoutGrid.transform);

        foreach (var res in building.GetInventory())
        {
            resourceQuantities[res.Key] = res.Value.Current;
        }
        
        foreach (var inputType in building.InputTypes)
        {
            var resourceRow = Instantiate(resourceRowInputPrefab, inputLayoutGrid.transform);
            var resourceRowInput = resourceRow.GetComponent<ResourceRowInput>();
            resourceRowInput.ResourceType = inputType;
            resourceRowInput.resourceTypeText.text = inputType.ToString();
            resourceRowInput.quantity.text = resourceQuantities[inputType].ToString();
            _resourceRowsInput.Add(resourceRowInput);
        }
        
    }
    
    public void UpdateResourceQuantities(Building building)
    {
        foreach (var row in _resourceRowsInput)
        {
            var resType = row.ResourceType;
            var resValue = building.GetInventory().FirstOrDefault(res => res.Key == resType).Value;
            row.UpdateQuantity(resValue.Current);
            row.UpdateInIncomingJobs(resValue.Incoming);
        }
    }
    
    public void DeactivateFoundationStateBuildingView()
    {
        // Destroy all child objects (rows) of the outputLayoutGrid
        foreach (Transform child in inputLayoutGrid.transform)
        {
            Destroy(child.gameObject);
        }
        _resourceRowsInput = new List<ResourceRowInput>();
        foundationStateBuildingView.gameObject.SetActive(false);
    }
}
