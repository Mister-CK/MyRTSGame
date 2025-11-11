using Interface;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using View.Components.Panels.SelectionPanels;

namespace Presentation
{
    public class SelectionPresenter
    {
        private VisualElement _bottomContainer;
        private SelectionPanel _activePanel;

        private Dictionary<System.Type, SelectionPanel> _panels;
        private ISelectable _currentSelection;
        private ISelectable _previousSelection;
        public SelectionPresenter(VisualElement bottomContainer, Dictionary<Type, SelectionPanel> panelsMap)
        {
            _bottomContainer = bottomContainer;
            _panels = panelsMap;
        }

        public void SetSelectedObject(ISelectable selectable)
        {
            _previousSelection = _currentSelection;
            _currentSelection = selectable;

            // 1. Clear the previous view if one existed
            if (_activePanel != null && _previousSelection != selectable)
            {
                _activePanel.ClearView();
                _activePanel = null;
            }

            // 2. Determine the correct panel based on the selected object's type
            if (_currentSelection == null)
            {
                return; // Selection cleared
            }

            var targetPanel = GetPanelForSelectable(_currentSelection);

            if (targetPanel != null)
            {
                _activePanel = targetPanel;
                _activePanel.SetView(_currentSelection);
            }
            else
            {
                // Fallback for unhandled types
                Debug.LogWarning($"No selection panel found for type: {_currentSelection.GetType().Name}");
            }
        }

        private SelectionPanel GetPanelForSelectable(ISelectable selectable)
        {
            if (selectable == null) return null;

            var type = selectable.GetType();

            // This handles inherited types (e.g., Warehouse will return BuildingSelectionPanel)
            foreach (var kvp in _panels)
            {
                if (kvp.Key.IsAssignableFrom(type))
                {
                    return kvp.Value;
                }
            }
            return null;
        }

        // --- Runtime Logic: Update Loop ---
        // This should be called from the Service (SelectionController)
        // private void LateUpdate()
        // {
        //     // 4. Update the active panel's dynamic content every frame
        //     if (_activePanel != null && _currentSelection != null)
        //     {
        //         _activePanel.UpdateView(_currentSelection);
        //     }
        // }
    }
}