using Buildings.Model;
using Buildings.Model.BuildingGroups;
using Enums;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using View.Components.Panels;
using View.Extensions;

namespace View.Components.Inventory
{
    /// <summary>
    /// A reusable UI component for displaying a list of resources in a 4-column table:
    /// [Resource Name] | [- Button] | [Target Amount] | [+ Button]
    /// </summary>
    public class ProductionTableComponent : VisualElement
    {
        private VisualElement _tableBody;
        private Label _header;
        
        // Cache for existing rows to allow efficient data updating
        private Dictionary<ResourceType, Label> _currentRows = new ();

        public ProductionTableComponent()
        {
            style.flexDirection = FlexDirection.Column;
            style.marginBottom = 10;
            
            // 1. Header
            _header = this.CreateChild<Label>("resource-table-header");
            _header.style.unityFontStyleAndWeight = UnityEngine.FontStyle.Bold;
            _header.style.fontSize = 16;
            _header.style.paddingBottom = 5;
            _header.style.borderBottomWidth = 1;
            _header.style.borderBottomColor = new UnityEngine.Color(0.8f, 0.8f, 0.8f);

            // 2. Table Header (4 Columns: Name | Reduce | Target | Add)
            var tableHeaderRow = CreateRow("table-header-row", true);
            
            var nameHeader = tableHeaderRow.CreateChild<Label>("name-header");
            nameHeader.text = "Resource";
            nameHeader.style.flexGrow = 1;

            var reduceHeader = tableHeaderRow.CreateChild<Label>("reduce-header");
            reduceHeader.text = "-";
            reduceHeader.style.width = new StyleLength(new Length(15, LengthUnit.Percent)); 
            reduceHeader.style.unityTextAlign = UnityEngine.TextAnchor.MiddleCenter;

            var targetHeader = tableHeaderRow.CreateChild<Label>("target-header");
            targetHeader.text = "Queue";
            targetHeader.style.width = new StyleLength(new Length(30, LengthUnit.Percent));
            targetHeader.style.unityTextAlign = UnityEngine.TextAnchor.MiddleRight;

            var addHeader = tableHeaderRow.CreateChild<Label>("add-header");
            addHeader.text = "+";
            addHeader.style.width = new StyleLength(new Length(15, LengthUnit.Percent)); 
            addHeader.style.unityTextAlign = UnityEngine.TextAnchor.MiddleCenter;

            this.Add(tableHeaderRow);

            // 3. Table Body Container
            _tableBody = this.CreateChild("table-body");
            _tableBody.style.flexDirection = FlexDirection.Column;
            _tableBody.pickingMode = PickingMode.Ignore; 

        }

        public void SetHeaderText(string headerText)
        {
            _header.text = headerText;
        }

        /// <summary>
        /// Initializes the production table by creating and caching all necessary UI elements (rows, buttons).
        /// This should only be called when the *view* is set, not every update.
        /// </summary>
        public void SetProduction(SelectionPanel selectionPanel, Dictionary<ResourceType, InventoryData> inventory, ResourceType[] outputTypes)
        {
            // Clear existing UI elements and the cache
            _tableBody.Clear();
            _currentRows.Clear();
            
            // Filter resources to only those that are relevant output types
            var relevantResources = inventory
                .Where(item => outputTypes != null && outputTypes.Any(resType => resType.Equals(item.Key)))
                .ToList();
            
            foreach (var item in relevantResources)
            {
                var resourceType = item.Key; 
                
                var row = CreateRow("data-row");
                
                // Column 1: Name
                var nameLabel = row.CreateChild<Label>("item-name");
                nameLabel.text = item.Key.ToString();
                nameLabel.style.minWidth = 0;
                nameLabel.style.flexGrow = 1;
                
                // Column 2: Reduce Button (20%)
                var reduceButton = row.CreateChild<Button>("production-reduce");
                reduceButton.text = "-";
                reduceButton.style.width = new StyleLength(new Length(15, LengthUnit.Percent)); 
                reduceButton.style.height = 24; 
                reduceButton.style.minWidth = 30; 
                reduceButton.style.flexShrink = 0; 
                reduceButton.style.unityTextAlign = UnityEngine.TextAnchor.MiddleCenter;
                
                // Attach persistent event listener
                reduceButton.clicked += () =>
                {
                    selectionPanel.ReduceMethod(resourceType);
                };

                // Column 3: Production Queue (30%) - This is the element that will be updated
                var outgoingLabel = row.CreateChild<Label>("item-production-queue");
                // Initial data set
                outgoingLabel.text = item.Value.InJob.ToString(); 
                outgoingLabel.style.width = new StyleLength(new Length(30, LengthUnit.Percent));
                outgoingLabel.style.flexShrink = 0; 
                outgoingLabel.style.unityTextAlign = UnityEngine.TextAnchor.MiddleRight;
                
                // Column 4: Add Button (20%)
                var addButton = row.CreateChild<Button>("production-add");
                addButton.text = "+";
                addButton.style.width = new StyleLength(new Length(15, LengthUnit.Percent));
                addButton.style.height = 24; 
                addButton.style.minWidth = 30; 
                addButton.style.flexShrink = 0; 
                addButton.style.unityTextAlign = UnityEngine.TextAnchor.MiddleCenter;
                
                // Attach persistent event listener
                addButton.clicked += () => 
                {
                    Debug.Log("add clicked for " + resourceType);
                    selectionPanel.AddMethod(resourceType);
                };
                
                _tableBody.Add(row);

                // Cache the row data for quick updates
                _currentRows.Add(resourceType, outgoingLabel);
            }
        }
        
        /// <summary>
        /// Updates the text data for existing production rows. 
        /// This is designed to be called efficiently in the Update cycle.
        /// </summary>
        public void UpdateProduction(Dictionary<ResourceType, InventoryData> inventory, WorkshopBuilding workshopBuilding)
        {
            if (_currentRows.Count == 0) return;

            foreach (var row in _currentRows)
            {
                row.Value.text = (workshopBuilding.ProductionJobs.Find(el => el.Output.ResourceType == row.Key).Quantity).ToString();
            }
        }


        private VisualElement CreateRow(string name, bool isHeader = false)
        {
            var row = new VisualElement();
            row.name = name;
            // Allow events to pass through this container to the buttons
            row.pickingMode = PickingMode.Ignore; 
            
            row.style.flexDirection = FlexDirection.Row;
            row.style.alignItems = Align.Center;
            row.style.paddingTop = isHeader ? 0 : 3;
            row.style.paddingBottom = isHeader ? 3 : 3;
            // Add a subtle line between data rows
            row.style.borderBottomWidth = isHeader ? 0 : 1;
            row.style.borderBottomColor = new UnityEngine.Color(0.95f, 0.95f, 0.95f);
            
            // Apply font style based on header status
            row.style.unityFontStyleAndWeight = isHeader ? UnityEngine.FontStyle.Bold : UnityEngine.FontStyle.Normal;

            return row;
        }
    }
}