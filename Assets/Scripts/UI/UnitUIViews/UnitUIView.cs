using TMPro;
using UnityEngine.UI;
using UnityEngine;

namespace MyRTSGame.Model.UnitViews
{
    public class UnitUIView : MonoBehaviour
    {
        [SerializeField] private Image unitUIView;
        [SerializeField] private TextMeshProUGUI unitName;
        [SerializeField] private GameObject buildingButton;
        [SerializeField] private GameObject staminaBarPrefab; 
        [SerializeField] private GameObject statusBars;
        private Slider _slider;
        
        private void SetBuildingButton(Unit unit)
        {
            if (unit is not ResourceCollector resourceCollector)
            {
                buildingButton.SetActive(false);
                return;
            }
            buildingButton.SetActive(resourceCollector.GetBuilding() != null);
        }

        public void ActivateView(Unit unit)
        {
            unitUIView.gameObject.SetActive(true);
            SetBuildingButton(unit);
            unitName.text = unit.GetUnitType().ToString();
            var bar = Instantiate(staminaBarPrefab, statusBars.transform);
            _slider = bar.GetComponent<Slider>();
            _slider.value = unit.GetStamina();
        }

        public void DeactivateView()
        {
          
            unitUIView.gameObject.SetActive(false);
            if (statusBars)
            {
                foreach (Transform child in statusBars.transform)
                {
                    Destroy(child.gameObject);
                }
            }
        }

        public void UpdateView(Unit unit)
        {
            SetBuildingButton(unit);
            _slider.value = unit.GetStamina();
        }
    }
}