using MyRTSGame.Model.ResourceSystem.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyRTSGame.Model
{
    public class NaturalResourceUIView: MonoBehaviour
    {
        [SerializeField] private Image resourceUIView;
        [SerializeField] private TextMeshProUGUI resourceType;
        [SerializeField] private GameObject statusBarPrefab; 
        [SerializeField] private GameObject statusBars;
        
        private Slider _slider;
        
        public void ActivateView(NaturalResource naturalResource)
        {
            resourceUIView.gameObject.SetActive(true);
            resourceType.text = naturalResource.GetResource().ToString();
            var bar = Instantiate(statusBarPrefab, statusBars.transform);
            _slider = bar.GetComponent<Slider>();
            _slider.value = 100; //todo: replace with time to to completion
            //_slider.value = unit.GetStamina();
        }
        
        public void DeactivateView()
        {
          
            resourceUIView.gameObject.SetActive(false);
            if (statusBars)
            {
                foreach (Transform child in statusBars.transform)
                {
                    Destroy(child.gameObject);
                }
            }
        }
        
        public void UpdateView(NaturalResource naturalResource)
        {
            //_slider.value = naturalResource.GetStamina();
        }
    }
}