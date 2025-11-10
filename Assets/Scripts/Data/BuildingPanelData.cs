using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [System.Serializable]
    public struct BuildableConfig
    {
        [Tooltip("The actual prefab that will be placed in the game.")]
        public GameObject prefab; 
    
        [Tooltip("The user-friendly name displayed on the UI button.")]
        public string displayName; 
    }

    [CreateAssetMenu(fileName = "NewBuildingData", menuName = "Game Data/Building Configuration")]
    public class BuildingPanelData : ScriptableObject
    {
        // The order of elements in this list controls the order of buttons in the UI.
        public List<BuildableConfig> buildings = new List<BuildableConfig>();
        public List<BuildableConfig> terrains = new List<BuildableConfig>();

    }

}
