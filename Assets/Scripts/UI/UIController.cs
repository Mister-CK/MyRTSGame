using MyRTSGame.Model;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject BuildPanel; 
    [SerializeField] private GameObject JobsPanel; 
    [SerializeField] private GameObject StatsPanel;
    [SerializeField] private GameObject MenuPanel; 
    [SerializeField] private GameObject SelectedPanel; 

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
        Debug.Log(panelType);
        BuildPanel.SetActive(panelType  == PanelType.Build);
        JobsPanel.SetActive(panelType  == PanelType.Jobs);
        StatsPanel.SetActive(panelType  == PanelType.Stats);
        MenuPanel.SetActive(panelType  == PanelType.Menu);
        SelectedPanel.SetActive(panelType  == PanelType.Selected);
    }
}