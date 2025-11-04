// using Buildings.Model;
// using System.Collections.Generic; // Your building model namespace
// using UnityEngine;
// using UnityEngine.UIElements;
//
// namespace UIToolkit.Services.ComponentGenerators
// {
//
//
//     public class BuildMenuGenerator : MonoBehaviour
//     {
//         [Tooltip("Drag the BuildButton.uxml template here.")] [SerializeField] private VisualTreeAsset buildButtonTemplate;
//
//         [Tooltip("The list of all available building data.")] [SerializeField] private List<Building> allBuildingTypes;
//
//         // This is the container in your UXML where buttons will be injected (e.g., inside 'panel-buildings')
//         private VisualElement _buttonGridContainer;
//
//         // --- You would call this method after activating the 'Buildings' tab ---
//         public void GenerateBuildButtons(VisualElement parentContainer)
//         {
//             _buttonGridContainer = parentContainer;
//             _buttonGridContainer.Clear(); // Clear any existing buttons
//
//             if (buildButtonTemplate == null)
//             {
//                 Debug.LogError("BuildButton UXML template is missing.");
//                 return;
//             }
//
//             foreach (var building in allBuildingTypes)
//             {
//                 // 1. CLONE THE TEMPLATE: Creates a new instance of the UXML structure
//                 VisualElement buttonInstance = buildButtonTemplate.CloneTree();
//
//                 // 2. QUERY THE PARTS: Get references to the elements inside the cloned instance
//                 Button button = buttonInstance.Q<Button>("build-button");
//                 Label label = button.Q<Label>("button-label");
//                 VisualElement icon = button.Q<VisualElement>("button-icon");
//
//                 // 3. INJECT DATA
//                 label.text = building.GetBuildingType().ToString(); // Or format with cost
//
//                 // Set the icon image (Assuming your Building class has a method to get the icon)
//                 icon.style.backgroundImage = new StyleBackground(building.Icon);
//
//                 // 4. BIND LOGIC: Attach the click handler, passing the specific building data
//                 button.clicked += () => HandleBuildButtonClick(building);
//
//                 // 5. INJECT INTO GRID
//                 _buttonGridContainer.Add(buttonInstance);
//             }
//         }
//
//         private void HandleBuildButtonClick(Building selectedBuilding)
//         {
//             Debug.Log($"Selected building to construct: {selectedBuilding.GetBuildingType()}");
//             // Your logic to enter 'placement mode' goes here
//         }
//     }
// }