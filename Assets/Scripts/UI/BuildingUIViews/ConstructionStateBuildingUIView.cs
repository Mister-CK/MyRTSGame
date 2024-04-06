using MyRTSGame.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ConstructionStateBuildingUIView : MonoBehaviour
{
    [SerializeField] private Image constructionStateBuildingView;
    [SerializeField] private TextMeshProUGUI constructionStateBuildingName;
    
    public void ActivateConstructionStateBuildingView(Building building)
    {
        constructionStateBuildingView.gameObject.SetActive(true);
        constructionStateBuildingName.text = building.BuildingType.ToString();
    }
    
    public void DeactivateConstructionStateBuildingView()
    {
        constructionStateBuildingView.gameObject.SetActive(false);
    }
}
