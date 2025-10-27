using Buildings.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace UI.BuildingUIViews
{
    public class ConstructionStateBuildingUIView : MonoBehaviour
    {
        [SerializeField] private Image constructionStateBuildingView;
        [SerializeField] private TextMeshProUGUI constructionStateBuildingName;

        public void ActivateConstructionStateBuildingView(Building building)
        {
            constructionStateBuildingView.gameObject.SetActive(true);
            constructionStateBuildingName.text = building.GetBuildingType().ToString();
        }

        public void DeactivateConstructionStateBuildingView()
        {
            constructionStateBuildingView.gameObject.SetActive(false);
        }
    }
}