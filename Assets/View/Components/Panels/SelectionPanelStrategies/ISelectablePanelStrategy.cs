using Interface;
using UnityEngine.UIElements;

namespace View.Components.Panels.SelectionPanelStrategies
{
    // Interface for all specific object type panel logic.
    public interface ISelectablePanelStrategy
    {
        // Builds the specific UI elements within the panel's root container.
        void Build(VisualElement rootContainer);
    
        // Sets up the view when an object of this type is selected.
        void SetView(ISelectable selectable);
    
        // Updates the view with dynamic data (called frequently in UpdateView).
        void UpdateView(ISelectable selectable);
    
        // Hides all UI elements specific to this strategy.
        void ClearView();
        }
}
