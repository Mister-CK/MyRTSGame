using System.Collections.Generic;
using System.Linq;
using MyRTSGame.Interface;
using MyRTSGame.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager: MonoBehaviour
{
    public static SelectionManager Instance;
    public ISelectable CurrentSelectedObject { get; private set; }
    
    [SerializeField] private Button deleteButton;
    [SerializeField] private TextMeshProUGUI textComponent;

    private void Awake()
    {
        // Ensure there is only one instance of SelectionManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void SelectObject(ISelectable newObject)
    {
        Debug.Log("selected object");
        if (CurrentSelectedObject != null)
        {
            // You can add code here to hide the details of the previously selected object
        }

        CurrentSelectedObject = newObject;
    }
    private void SetDeleteButton(bool show)
    {
        deleteButton.gameObject.SetActive(show);
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            CurrentSelectedObject = null;
        }
        
        if (CurrentSelectedObject == null)
        {
            textComponent.text = "";
            SetDeleteButton(false);
            return;
        }
        if (CurrentSelectedObject is Building building)
        {
            var text = building.BuildingType + "\n" +
                       GetTextForInventory(building.GetInventory()) + "\n" +
                       building.GetState() + "\n" +
                       GetTextForInputTypes(building.InputTypes) + "\n" +
                       building.GetCapacity();
            textComponent.text = text;
            SetDeleteButton(true);
            return;
        }
        
        if (CurrentSelectedObject is Villager villager)
        {
            textComponent.text = "Villager";
            return;
        }
    }
    
    private static string GetTextForInputTypes(IEnumerable<ResourceType> inputTypes) 
    {
        return inputTypes.Aggregate("input types: ", (current, resourceType) => current + resourceType + ", ").TrimEnd(',', ' ');
    }
    
    private static string GetTextForInventory(Resource[] inventory) {
        string inventoryText = "";
        foreach (Resource resource in inventory) {
            inventoryText += resource.ResourceType + ":" + resource.Quantity + " ";
        }
        return inventoryText;
    }
}