using TMPro;
using Units.Model.Component;
using UnityEngine.UI;
using UnityEngine;

namespace UI.UnitUIViews
{
    public class UnitUIView : MonoBehaviour
    {
        [SerializeField] private Image unitUIView;
        [SerializeField] private TextMeshProUGUI unitName;
        [SerializeField] private GameObject buildingButton;
        [SerializeField] private GameObject staminaBarPrefab; 
        [SerializeField] private GameObject statusBars;
        
        private Slider _slider;
        
        private void SetBuildingButton(UnitComponent unit)
        {
            if (unit is not ResourceCollectorComponent resourceCollector)
            {
                buildingButton.SetActive(false);
                return;
            }
            buildingButton.SetActive(resourceCollector.CollectorData.Building != null);
        }

        public void ActivateView(UnitComponent unit)
        {
            unitUIView.gameObject.SetActive(true);
            SetBuildingButton(unit);
            unitName.text = unit.Data.UnitType.ToString();
            var bar = Instantiate(staminaBarPrefab, statusBars.transform);
            _slider = bar.GetComponent<Slider>();
            _slider.value = unit.Data.GetStamina();
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

        public void UpdateView(UnitComponent unit)
        {
            SetBuildingButton(unit);
            _slider.value = unit.Data.GetStamina();
        }
    }
}