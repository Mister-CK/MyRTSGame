using Buildings.Model;
using Enums;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;
using View.Extensions;

namespace View.Components.Inventory
{
    /// <summary>
    /// A reusable UI component for displaying a list of resources in a 3-column table:
    /// [Resource Name] | [Current Amount] | [Target/Flow Amount]
    /// </summary>
    public class ResourceTableComponent : VisualElement
    {
        private VisualElement _tableBody;
        private Label _header;

        private Label _targetHeader;
        // Caches to hold references to the labels that need real-time updates
        private Dictionary<ResourceType, Label> _currentLabelCache = new Dictionary<ResourceType, Label>();
        private Dictionary<ResourceType, Label> _targetLabelCache = new Dictionary<ResourceType, Label>();

        public ResourceTableComponent()
        {
            // Set up main component styling
            style.flexDirection = FlexDirection.Column;
            style.marginBottom = 10;
            
            // 1. Header
            _header = this.CreateChild<Label>("resource-table-header");
            _header.style.unityFontStyleAndWeight = UnityEngine.FontStyle.Bold;
            _header.style.fontSize = 16;
            _header.style.paddingBottom = 5;
            _header.style.borderBottomWidth = 1;
            _header.style.borderBottomColor = new UnityEngine.Color(0.8f, 0.8f, 0.8f);

            // 2. Table Header (3 Columns)
            var tableHeaderRow = CreateRow("table-header-row", true);
            
            var nameHeader = tableHeaderRow.CreateChild<Label>("name-header");
            nameHeader.text = "Resource";
            nameHeader.style.flexGrow = 1;

            var currentHeader = tableHeaderRow.CreateChild<Label>("current-header");
            currentHeader.text = "Current";
            // Use percentages for responsive column widths
            currentHeader.style.width = new StyleLength(new Length(30, LengthUnit.Percent)); 
            currentHeader.style.unityTextAlign = UnityEngine.TextAnchor.MiddleRight;

            _targetHeader = tableHeaderRow.CreateChild<Label>("target-header");
            _targetHeader.text = "Target";
            _targetHeader.style.width = new StyleLength(new Length(30, LengthUnit.Percent));
            _targetHeader.style.unityTextAlign = UnityEngine.TextAnchor.MiddleRight;

            this.Add(tableHeaderRow);

            // 3. Table Body Container
            _tableBody = this.CreateChild("table-body");
            _tableBody.style.flexDirection = FlexDirection.Column;
        }

        public void SetHeaderText(string headerText)
        {
            _header.text = headerText;
        }
        
        public void SetTargetLabel(string targetLabel)
        {
            _targetHeader.text = targetLabel;
        }

        /// <summary>
        /// Initializes the table for displaying Input resources (Current & Incoming). 
        /// Creates UI elements and caches the labels.
        /// </summary>
        public void SetInput(Dictionary<ResourceType, InventoryData> inventory, ResourceType[] inputTypes)
        {
            _tableBody.Clear();
            _currentLabelCache.Clear();
            _targetLabelCache.Clear();

            var relevantResources = inventory
                .Where(item => inputTypes != null && inputTypes.Any(resType => resType.Equals(item.Key)))
                .ToList();
            
            foreach (var item in relevantResources)
            {
                var resourceType = item.Key;
                var row = CreateRow("data-row");
                
                // Column 1: Name
                var nameLabel = row.CreateChild<Label>("item-name");
                nameLabel.text = resourceType.ToString();
                nameLabel.style.flexGrow = 1;

                // Column 2: Current Amount (Caches this label)
                var currentLabel = row.CreateChild<Label>("item-current");
                currentLabel.text = item.Value.Current.ToString();
                currentLabel.style.width = new StyleLength(new Length(30, LengthUnit.Percent));
                currentLabel.style.unityTextAlign = UnityEngine.TextAnchor.MiddleRight;
                _currentLabelCache.Add(resourceType, currentLabel);
                
                // Column 3: Incoming Amount (Caches this label)
                var incomingLabel = row.CreateChild<Label>("item-incoming");
                incomingLabel.text = item.Value.Incoming.ToString();
                incomingLabel.style.width = new StyleLength(new Length(30, LengthUnit.Percent));
                incomingLabel.style.unityTextAlign = UnityEngine.TextAnchor.MiddleRight;
                _targetLabelCache.Add(resourceType, incomingLabel);
                
                _tableBody.Add(row);
            }
        }
        
        /// <summary>
        /// Initializes the table for displaying Output resources (Current & Outgoing).
        /// Creates UI elements and caches the labels.
        /// </summary>
        public void SetOutput(Dictionary<ResourceType, InventoryData> inventory, ResourceType[] outputTypes)
        {
            _tableBody.Clear();
            _currentLabelCache.Clear();
            _targetLabelCache.Clear();
            
            var relevantResources = inventory
                .Where(item => outputTypes != null && outputTypes.Any(resType => resType.Equals(item.Key)))
                .ToList();

            foreach (var item in relevantResources)
            {
                var resourceType = item.Key;
                var row = CreateRow("data-row");

                // Column 1: Name
                var nameLabel = row.CreateChild<Label>("item-name");
                nameLabel.text = resourceType.ToString();
                nameLabel.style.flexGrow = 1;

                // Column 2: Current Amount (Caches this label)
                var currentLabel = row.CreateChild<Label>("item-current");
                currentLabel.text = item.Value.Current.ToString();
                currentLabel.style.width = new StyleLength(new Length(30, LengthUnit.Percent));
                currentLabel.style.unityTextAlign = UnityEngine.TextAnchor.MiddleRight;
                _currentLabelCache.Add(resourceType, currentLabel);

                // Column 3: Outgoing Amount (Caches this label)
                var outgoingLabel = row.CreateChild<Label>("item-outgoing");
                outgoingLabel.text = item.Value.Outgoing.ToString();
                outgoingLabel.style.width = new StyleLength(new Length(30, LengthUnit.Percent));
                outgoingLabel.style.unityTextAlign = UnityEngine.TextAnchor.MiddleRight;
                _targetLabelCache.Add(resourceType, outgoingLabel);
                
                _tableBody.Add(row);
            }
        }
        
        /// <summary>
        /// Efficiently updates the Current and Incoming resource data using the cached labels.
        /// This method is for Input tables.
        /// </summary>
        public void UpdateInputData(Dictionary<ResourceType, InventoryData> inventory)
        {
            if (_currentLabelCache.Count == 0) return;

            foreach (var kvp in _currentLabelCache)
            {
                var resourceType = kvp.Key;
                var currentLabel = kvp.Value;

                if (inventory.TryGetValue(resourceType, out InventoryData data))
                {
                    // Update Current amount
                    currentLabel.text = data.Current.ToString();

                    // Update Incoming amount
                    if (_targetLabelCache.TryGetValue(resourceType, out Label incomingLabel))
                    {
                        incomingLabel.text = data.Incoming.ToString();
                    }
                }
            }
        }
        
        /// <summary>
        /// Efficiently updates the Current and Outgoing resource data using the cached labels.
        /// This method is for Output tables.
        /// </summary>
        public void UpdateOutputData(Dictionary<ResourceType, InventoryData> inventory)
        {
            if (_currentLabelCache.Count == 0) return;

            foreach (var kvp in _currentLabelCache)
            {
                var resourceType = kvp.Key;
                var currentLabel = kvp.Value;

                if (inventory.TryGetValue(resourceType, out InventoryData data))
                {
                    // Update Current amount
                    currentLabel.text = data.Current.ToString();

                    // Update Outgoing amount
                    if (_targetLabelCache.TryGetValue(resourceType, out Label outgoingLabel))
                    {
                        outgoingLabel.text = data.Outgoing.ToString();
                    }
                }
            }
        }

        private VisualElement CreateRow(string name, bool isHeader = false)
        {
            var row = new VisualElement();
            row.name = name;
            row.style.flexDirection = FlexDirection.Row;
            row.style.paddingTop = isHeader ? 0 : 3;
            row.style.paddingBottom = 3;
            // Add a subtle line between data rows
            row.style.borderBottomWidth = isHeader ? 0 : 1;
            row.style.borderBottomColor = new UnityEngine.Color(0.95f, 0.95f, 0.95f);
            
            // Apply font style based on header status
            row.style.unityFontStyleAndWeight = isHeader ? UnityEngine.FontStyle.Bold : UnityEngine.FontStyle.Normal;

            return row;
        }
    }
}