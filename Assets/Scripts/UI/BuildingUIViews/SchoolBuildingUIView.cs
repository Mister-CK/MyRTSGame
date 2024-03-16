using System.Collections.Generic;
using MyRTSGame.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SchoolBuildingUIView : MonoBehaviour
{
    [SerializeField] private Image schoolBuildingView;
    [SerializeField] private TextMeshProUGUI schoolBuildingName;
    [SerializeField] private GameObject inputTitlePrefab;
    [SerializeField] private GameObject columnsPrefab;
    [SerializeField] private GameObject resourceRowPrefab;
    [SerializeField] private GameObject inputLayoutGrid;
    
    [SerializeField] private GameEvent onNewVillagerEvent;

    private School _currentSchool;
    
    private List<ResourceRowOutput> _resourceRowOutputs = new List<ResourceRowOutput>();
    public void ActivateSchoolBuildingView(School school)
    {
        _currentSchool = school;
        schoolBuildingView.gameObject.SetActive(true);
        schoolBuildingName.text = school.BuildingType.ToString();
        Instantiate(inputTitlePrefab, inputLayoutGrid.transform);
        Instantiate(columnsPrefab, inputLayoutGrid.transform);

        foreach (var res in school.InventoryWhenCompleted)
        {
            var resourceRow = Instantiate(resourceRowPrefab, inputLayoutGrid.transform);
            var resourceRowOutput = resourceRow.GetComponent<ResourceRowOutput>();
            resourceRow.GetComponent<ResourceRowOutput>().ResourceType.text = res.ResourceType.ToString();
            resourceRow.GetComponent<ResourceRowOutput>().Quantity.text = res.Quantity.ToString();
            _resourceRowOutputs.Add(resourceRowOutput);
        }
    }
    
    public void UpdateResourceQuantities(School school)
    {
        for (var i = 0; i < _resourceRowOutputs.Count; i++)
        {
            _resourceRowOutputs[i].UpdateQuantity(school.InventoryWhenCompleted[i].Quantity);
        }
    }
    
    public void DeactivateSchoolBuildingView()
    {
        // Destroy all child objects (rows) of the outputLayoutGrid
        foreach (Transform child in inputLayoutGrid.transform)
        {
            Destroy(child.gameObject);
        }
        _resourceRowOutputs = new List<ResourceRowOutput>();
        schoolBuildingView.gameObject.SetActive(false);
    }
    
    public void OnNewVillButtonClick()
    {
        Debug.Log("New villager button clicked" + _currentSchool.transform.position);
        onNewVillagerEvent.Raise(new SchoolEventArgs(_currentSchool));
    }
}
