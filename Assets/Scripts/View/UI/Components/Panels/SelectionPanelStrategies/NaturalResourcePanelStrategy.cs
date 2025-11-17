using Enums;
using Interface;
using Domain.Model.ResourceSystem.Model;
using Domain.Model.ResourceSystem.Model.ResourceStates;
using System;
using UnityEngine.UIElements;
using View.Extensions;

namespace View.Components.Panels.SelectionPanelStrategies
{
    public class NaturalResourcePanelStrategy : ISelectablePanelStrategy
    {
        // --- CACHED RESOURCE UI ELEMENTS ---
        private VisualElement _contentRoot;
        private Label _resourceNameLabel; 
        private VisualElement _statusBarContainer; 
        private Slider _growthSlider; 
        private Label _resourceQuantityLabel; 
        private Type _lastKnownStateType;
        private ResourceType _primaryResourceType;

        private SelectionPanel _selectionPanel;
        
        public void Build(SelectionPanel selectionPanel, VisualElement rootContainer)
        {
            _contentRoot = rootContainer.CreateChild("resource-panel-content");
            _selectionPanel = selectionPanel;
            
            _resourceNameLabel = _contentRoot.CreateChild<Label>("resource-name-label");
            _resourceNameLabel.AddToClassList("selection-panel-header");

            _statusBarContainer = _contentRoot.CreateChild("resource-status-bar-container");
            _statusBarContainer.style.flexDirection = FlexDirection.Column;
            
            _growthSlider = new Slider(0, 100, SliderDirection.Horizontal, 1f);
            _growthSlider.name = "growth-slider";
            _growthSlider.label = "Growth Progress";
            _growthSlider.SetEnabled(false); 
            _statusBarContainer.Add(_growthSlider);
            
            var resourceGrid = _contentRoot.CreateChild("resource-grid");
            resourceGrid.style.flexDirection = FlexDirection.Row;
            resourceGrid.style.justifyContent = Justify.SpaceBetween;
            
            var typeLabel = resourceGrid.CreateChild<Label>("resource-type-text");
            typeLabel.text = "Quantity:"; 
            
            _resourceQuantityLabel = resourceGrid.CreateChild<Label>("resource-quantity-label");
            _resourceQuantityLabel.AddToClassList("quantity-value");

            _contentRoot.style.display = DisplayStyle.None;
        }

        public void SetView(ISelectable selectable)
        {
            if (selectable is not NaturalResource resource) return;
            if (resource == null) return; 
            _contentRoot.style.display = DisplayStyle.Flex;
            
            _primaryResourceType = resource.GetResourceType();
            _resourceNameLabel.text = resource.name;

            if (resource.GetState() is GrowingState) _statusBarContainer.style.display = DisplayStyle.Flex;
            else _statusBarContainer.style.display = DisplayStyle.None;
            
            UpdateView(resource);
        }

        public void UpdateView(ISelectable selectable)
        {
            if (selectable is not NaturalResource resource) return;
            if (resource == null) return; 

            if (resource.GetState() is GrowingState growingState)
            {
                _growthSlider.value = growingState.GetPercentageGrown(); 
            }
            
            var inventory = resource.GetInventory();
            if (inventory.ContainsKey(_primaryResourceType))
            {
                var quantity = inventory[_primaryResourceType].Current;
                _resourceQuantityLabel.text = quantity.ToString();
            }
        }

        public void ClearView()
        {
            if (_contentRoot != null) _contentRoot.style.display = DisplayStyle.None;
            _primaryResourceType = default; 
        }
        
        public bool HasStateChanged(NaturalResource naturalResource)
        {
            var currentState = naturalResource.GetState().GetType();
            return currentState != _lastKnownStateType;
        }
    }
}