using TMPro;
using UnityEngine.UI;
using UnityEngine;

namespace MyRTSGame.Model.UnitViews
{
    public class UnitUIView : MonoBehaviour
    {
        [SerializeField] private Image unitUIView;
        [SerializeField] private TextMeshProUGUI unitName;
        [SerializeField] private GameObject staminaBarPrefab; 
        [SerializeField] private GameObject statusBars;
        public void ActivateView(Unit unit)
        {
            unitUIView.gameObject.SetActive(true);
            unitName.text = unit.GetUnitType().ToString();
            Instantiate(staminaBarPrefab, statusBars.transform);

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
        }
    }
}