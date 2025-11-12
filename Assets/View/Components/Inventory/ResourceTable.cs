using Buildings.Model;
using Enums;
using MyRTSGame.Model;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using View.Extensions;

namespace View.Components.Inventory
{
    /// <summary>
    /// A reusable UI component for displaying a list of resources in a 3-column table:
    /// [Resource Name] | [Current Amount] | [Target Amount]
    /// </summary>
    public class ResourceTableComponent : VisualElement
    {
        private VisualElement _tableBody;
        private Label _header;
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

            var targetHeader = tableHeaderRow.CreateChild<Label>("target-header");
            targetHeader.text = "Target";
            targetHeader.style.width = new StyleLength(new Length(30, LengthUnit.Percent));
            targetHeader.style.unityTextAlign = UnityEngine.TextAnchor.MiddleRight;

            this.Add(tableHeaderRow);

            // 3. Table Body Container
            _tableBody = this.CreateChild("table-body");
            _tableBody.style.flexDirection = FlexDirection.Column;
        }

        public void SetHeaderText(string headerText)
        {
            _header.text = headerText;
        }

        public void SetInput(Dictionary<ResourceType, InventoryData> inventory, ResourceType[] inputTypes)
        {
            _tableBody.Clear();

            foreach (var item in inventory)
            {
                if (inputTypes != null && !inputTypes.Any(resType => resType.Equals(item.Key))) continue;
                var row = CreateRow("data-row");
                
                // Column 1: Name
                var nameLabel = row.CreateChild<Label>("item-name");
                nameLabel.text = item.Key.ToString();
                nameLabel.style.flexGrow = 1;

                // Column 2: Current Amount
                var currentLabel = row.CreateChild<Label>("item-current");
                currentLabel.text = item.Value.Current.ToString();
                currentLabel.style.width = new StyleLength(new Length(30, LengthUnit.Percent));
                currentLabel.style.unityTextAlign = UnityEngine.TextAnchor.MiddleRight;
                
                // Column 3: Incomming Amount
                var incommingLabel = row.CreateChild<Label>("item-incomming");
                incommingLabel.text = item.Value.Incoming.ToString();
                incommingLabel.style.width = new StyleLength(new Length(30, LengthUnit.Percent));
                incommingLabel.style.unityTextAlign = UnityEngine.TextAnchor.MiddleRight;
                _tableBody.Add(row);
            }

        }
        
        public void SetOutput(Dictionary<ResourceType, InventoryData> inventory, ResourceType[] outputTypes)
        {
            _tableBody.Clear();
            
            foreach (var item in inventory)
            {
                if (outputTypes != null && !outputTypes.Any(resType => resType.Equals(item.Key))) continue;

                var row = CreateRow("data-row");

                // Column 1: Name
                var nameLabel = row.CreateChild<Label>("item-name");
                nameLabel.text = item.Key.ToString();
                nameLabel.style.flexGrow = 1;

                // Column 2: Current Amount
                var currentLabel = row.CreateChild<Label>("item-current");
                currentLabel.text = item.Value.Current.ToString();
                currentLabel.style.width = new StyleLength(new Length(30, LengthUnit.Percent));
                currentLabel.style.unityTextAlign = UnityEngine.TextAnchor.MiddleRight;

                // Column 3: Outgoing Amount
                var incommingLabel = row.CreateChild<Label>("item-outgoing");
                incommingLabel.text = item.Value.Outgoing.ToString();
                incommingLabel.style.width = new StyleLength(new Length(30, LengthUnit.Percent));
                incommingLabel.style.unityTextAlign = UnityEngine.TextAnchor.MiddleRight;
                _tableBody.Add(row);
            }
        }

        private VisualElement CreateRow(string name, bool isHeader = false)
        {
            var row = new VisualElement();
            row.name = name;
            row.style.flexDirection = FlexDirection.Row;
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