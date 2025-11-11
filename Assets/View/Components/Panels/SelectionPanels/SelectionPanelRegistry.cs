using UnityEngine;
using UnityEngine.UIElements;
using Buildings.Model;
using System.Collections.Generic;

namespace View.Components.Panels.SelectionPanels
{
    public static class SelectionPanelRegistry
    {
        public static Dictionary<System.Type, SelectionPanel> CreateAndBuildPanels(VisualElement container)
        {
            var panels = new Dictionary<System.Type, SelectionPanel>();
            
            var buildingPanel = new BuildingSelectionPanel();
            panels.Add(typeof(Building), buildingPanel);
            

            foreach (var panel in panels.Values)
            {
                panel.Build(container);
            }

            Debug.Log($"SelectionPanelRegistry created and built {panels.Count} panel views.");
            return panels;
        }
    }
}