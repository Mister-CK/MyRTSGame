using Enums;
using Interface;
using UnityEngine;

namespace UI.Controller
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private GameObject buildPanel;
        [SerializeField] private GameObject jobsPanel;
        [SerializeField] private GameObject statsPanel;
        [SerializeField] private GameObject menuPanel;
        [SerializeField] private GameObject selectedPanel;

        [SerializeField] private GameEvent onDeselectionEvent;
        [SerializeField] private GameEvent onSelectionEvent;

        private void OnEnable()
        {
            onSelectionEvent.RegisterListener(HandleSelectionEvent);
            onDeselectionEvent.RegisterListener(HandleDeselectionEvent);
        }

        private void OnDisable()
        {
            onSelectionEvent.UnregisterListener(HandleSelectionEvent);
            onDeselectionEvent.UnregisterListener(HandleDeselectionEvent);
        }

        public void OnBuildButtonClick()
        {
            ActivateSelectedPanel(PanelType.Build);
        }

        public void OnJobsButtonClick()
        {
            ActivateSelectedPanel(PanelType.Jobs);
        }

        public void OnStatsButtonClick()
        {
            ActivateSelectedPanel(PanelType.Stats);
        }

        public void OnMenuButtonClick()
        {
            ActivateSelectedPanel(PanelType.Menu);
        }

        private void HandleSelectionEvent(IGameEventArgs args)
        {
            ActivateSelectedPanel(PanelType.Selected);
        }

        private void HandleDeselectionEvent(IGameEventArgs args)
        {
            ActivateSelectedPanel(null);
        }

        private void ActivateSelectedPanel(PanelType? panelType)
        {
            buildPanel.SetActive(panelType == PanelType.Build);
            jobsPanel.SetActive(panelType == PanelType.Jobs);
            statsPanel.SetActive(panelType == PanelType.Stats);
            menuPanel.SetActive(panelType == PanelType.Menu);
            selectedPanel.SetActive(panelType == PanelType.Selected);
        }
    }
}