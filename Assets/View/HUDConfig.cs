// HUDConfig.cs
using System.Collections.Generic;

namespace View
{
    /// <summary>
    /// Represents the immutable configuration for a single panel/button pair.
    /// </summary>
    public struct PanelConfiguration
    {
        public string Id { get; }      // Used for element name (e.g., "panel-build")
        public string Label { get; }   // Used for button text (e.g., "BUILD")
        public string ContentText { get; } // Text content for the panel

        public PanelConfiguration(string id, string label, string contentText)
        {
            Id = id;
            Label = label;
            ContentText = contentText;
        }
    }

    /// <summary>
    /// Centralized storage for all UI constants and configurations.
    /// </summary>
    public static class HUDConfig
    {
        public static readonly IReadOnlyList<PanelConfiguration> PanelConfigurations = new List<PanelConfiguration>
        {
            new PanelConfiguration("build", "BUILD", "BUILD PANEL: Select structures to place."),
            new PanelConfiguration("jobs", "JOBS", "JOBS PANEL: Manage villager assignments."),
            new PanelConfiguration("stats", "STATS", "STATS PANEL: View economy and population data."),
            new PanelConfiguration("menu", "MENU", "MENU PANEL: Save, Load, and Settings."),
        };
        
        // --- CSS Class Constants ---
        // Using constants prevents typos when calling AddToClassList
        public const string LEFT_PANEL_CLASS = "leftPanel";
        public const string TOP_CONTAINER_CLASS = "top-container";
        public const string COMMAND_PANEL_CLASS = "command-panel";
        public const string ACTIVE_BUTTON_CLASS = "active";
        public const string INACTIVE_PANEL_CLASS = "inactive-panel";
        
        // --- Element Name Conventions ---
        public static string GetButtonName(string panelId) => $"btn-{panelId}";
        public static string GetPanelName(string panelId) => $"panel-{panelId}";
    }
}
