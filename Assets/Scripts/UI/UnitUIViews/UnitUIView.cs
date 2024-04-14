using TMPro;
using UnityEngine.UI;
using UnityEngine;

namespace MyRTSGame.Model.UnitViews
{
    public class UnitUIView : MonoBehaviour
    {
        [SerializeField] private Image unitUIView;
        [SerializeField] private TextMeshProUGUI unitName;

        public void ActivateView(Unit unit)
        {
            unitUIView.gameObject.SetActive(true);
            unitName.text = unit.GetUnitType().ToString();
        }

        public void DeactivateView()
        {
            unitUIView.gameObject.SetActive(false);
        }

        public void UpdateView()
        {
        }
    }
}