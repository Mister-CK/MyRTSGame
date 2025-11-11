using Buildings.Model;
using Buildings.Model.BuildingStates;
using Enums;
using Interface;
using MyRTSGame.Model;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using View.Extensions;

namespace View.Components.Panels.SelectionPanels
{
    public class BuildingSelectionPanel : SelectionPanel
    {
        private Label _titleLabel;
        private Label _subtitleLabel;
        private VisualElement _dynamicCommandArea;
        private VisualElement _inventoryDisplay; 

        private static BuildingPlacer _buildingPlacer; 

        public BuildingSelectionPanel() : base(typeof(BuildingSelectionPanel).Name) 
        {
            if (_buildingPlacer == null)
            {
                // Find the Monobehaviour responsible for game actions once
                _buildingPlacer = UnityEngine.Object.FindObjectOfType<BuildingPlacer>();
            }
        }

        /// <summary>
        /// 1. Builds the static UI structure (Title, Health, Containers).
        /// </summary>
        protected override void OnBuild()
        {
            // 1. Title and Subtitle Area
            _titleLabel = Root.CreateChild<Label>("title-label", "detail-title");
            _subtitleLabel = Root.CreateChild<Label>("subtitle-label", "detail-subtitle");

            // 2. Health Bar / Progress Bar (Placeholder)
            Root.CreateChild<VisualElement>("health-bar-placeholder").style.height = 10;
            Root.Q("health-bar-placeholder").style.backgroundColor = new Color(0.2f, 0.8f, 0.2f);
            Root.Q("health-bar-placeholder").style.marginBottom = 10;

            // 3. Dynamic Command Area (Where buttons/state info are injected)
            _dynamicCommandArea = Root.CreateChild("dynamic-commands", "dynamic-content-area");

            // 4. Inventory/Resource Area (Optional, hidden by default)
            _inventoryDisplay = Root.CreateChild("inventory-display-area", "resource-list");
            _inventoryDisplay.style.display = DisplayStyle.None; 
        }

        /// <summary>
        /// 2. Logic to run when the panel is first shown for a specific building.
        /// </summary>
        protected override void OnActivateLogic(ISelectable selectable)
        {
            if (selectable is not Building building)
            {
                Debug.LogError("BuildingSelectionPanel activated with a non-Building selectable.");
                return;
            }

            // A. Update static labels
            _titleLabel.text = building.name.ToUpper();
            _subtitleLabel.text = $"Health: 100/100 (Placeholder)"; // Will be updated in UpdateView

            // B. Render state-specific commands
            RenderCommandsByState(building);
        }

        /// <summary>
        /// 3. Updates dynamic data every frame (e.g., health, construction progress).
        /// </summary>
        public override void UpdateView(ISelectable selectable)
        {
            if (selectable is not Building building) return;
            
            // Example update logic for health display
            // This logic can be expanded to check for state transitions
            var currentHealth = UnityEngine.Random.Range(50, 100); // Simulated dynamic data
            _subtitleLabel.text = $"Health: {currentHealth}/100";
            
            // If it's a Warehouse, update inventory display
            if (building is Warehouse warehouse)
            {
                UpdateInventoryDisplay(warehouse.GetInventory());
            }
        }
        
        /// <summary>
        /// Helper: Renders the appropriate UI for the building's current state.
        /// </summary>
        private void RenderCommandsByState(Building building)
        {
            _dynamicCommandArea.Clear();
            _inventoryDisplay.Clear();
            _inventoryDisplay.style.display = DisplayStyle.None;

            // Determine the state
            var state = building.GetState(); 

            // Always add a generic delete button if the building is placeable
            AddDeleteButton(building); 
            
            if (state is FoundationState)
            {
                RenderFoundationView(building);
            }
            else if (state is ConstructionState)
            {
                RenderConstructionView(building);
            }
            else if (building is Warehouse warehouse) // Check for specific type after construction state
            {
                RenderWarehouseView(warehouse);
            }
            else if (state is CompletedState)
            {
                RenderCompletedView(building);
            }
        }

        // --- View Rendering Helpers ---
        
        private void RenderFoundationView(Building building)
        {
            _dynamicCommandArea.Add(CreateCommandButton(
                label: $"Gather Cost (x)",
                cssClass: "command-button",
                onClick: () => { Debug.Log("Foundation: Requesting resources for construction."); }
            ));
        }

        private void RenderConstructionView(Building building)
        {
            _dynamicCommandArea.Add(new Label("Construction Progress: 50% (Placeholder)") 
            {
                style = { color = Color.yellow }
            });
            _dynamicCommandArea.Add(CreateCommandButton(
                label: "Prioritize Construction",
                cssClass: "command-button",
                onClick: () => { Debug.Log("Construction: Prioritizing this building."); }
            ));
        }

        private void RenderCompletedView(Building building)
        {
            _dynamicCommandArea.Add(CreateCommandButton(
                label: "Access Production Menu",
                cssClass: "command-button",
                onClick: () => { Debug.Log("Completed: Opening production UI."); }
            ));
            
            if (building.GetOccupant() != null)
            {
                 _dynamicCommandArea.Add(CreateCommandButton(
                    label: "View Occupant Unit",
                    cssClass: "command-button",
                    onClick: () => { Debug.Log("Completed: Select Unit inside."); }
                ));
            }
        }

        private void RenderWarehouseView(Warehouse warehouse)
        {
            RenderCompletedView(warehouse); // Warehouse is also completed, so render base commands first
            
            _dynamicCommandArea.Add(new Label("WAREHOUSE INVENTORY") 
            {
                style = { color = Color.white, marginTop = 10, fontSize = 14 }
            });
            
            _inventoryDisplay.style.display = DisplayStyle.Flex;
            UpdateInventoryDisplay(warehouse.GetInventory());
        }
        
        // --- Shared UI Helpers ---

        private void AddDeleteButton(Building building)
        {
            _dynamicCommandArea.Add(CreateCommandButton(
                label: "Demolish Building",
                cssClass: "command-button delete-button", // Use delete-button for red styling
                onClick: () => { 
                    Debug.Log($"Demolishing {building.name}.");
                    // Implement confirmation dialog logic here instead of Debug.Log
                }
            ));
        }
        
        private Button CreateCommandButton(string label, string cssClass, Action onClick)
        {
            var btn = new Button(onClick) { text = label };
            btn.AddToClassList(cssClass);
            return btn;
        }
        
        private void UpdateInventoryDisplay(Dictionary<ResourceType, InventoryData> inventory)
        {
            _inventoryDisplay.Clear();

            foreach (var kvp in inventory)
            {
                var label = new Label($"{kvp.Key}: {kvp.Value}");
                label.AddToClassList("resource-label");
                _inventoryDisplay.Add(label);
            }
        }
    }
}