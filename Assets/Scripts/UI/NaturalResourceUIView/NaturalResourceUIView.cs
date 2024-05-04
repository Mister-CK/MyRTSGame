using System.Collections.Generic;
using MyRTSGame.Model.ResourceSystem.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyRTSGame.Model
{
    public class NaturalResourceUIView: MonoBehaviour
    {
        [SerializeField] private Image resourceUIView;
        [SerializeField] private TextMeshProUGUI naturalResourceName;
        [SerializeField] private GameObject statusBarPrefab; 
        [SerializeField] private GameObject statusBars;
        
        [SerializeField] private GameObject resourceGrid;
        [SerializeField] private GameObject resourceGridTitle;
        [SerializeField] private GameObject resourceRowPrefab;
        [SerializeField] private GameObject columnsPrefab;
        private List<ResourceRowOutput> _resourceRows = new ();

        private Slider _slider;
        
        public void ActivateView(NaturalResource naturalResource)
        {
            resourceUIView.gameObject.SetActive(true);
            naturalResourceName.text = naturalResource.name;
            var bar = Instantiate(statusBarPrefab, statusBars.transform);
            _slider = bar.GetComponent<Slider>();
            _slider.value = 100; //todo: replace with time to to completion
            //_slider.value = unit.GetStamina();
            var resource = naturalResource.GetResource();
            
            Instantiate(resourceGridTitle, resourceGrid.transform);
            Instantiate(columnsPrefab, resourceGrid.transform);

            var resourceRow = Instantiate(resourceRowPrefab, resourceGrid.transform);
            var resourceRowOutput = resourceRow.GetComponent<ResourceRowOutput>();
            resourceRowOutput.ResourceType = resource.ResourceType;
            resourceRowOutput.resourceTypeText.text = resource.ResourceType.ToString();
            resourceRowOutput.quantity.text = resource.Quantity.ToString();
            _resourceRows.Add(resourceRowOutput);
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
            if (resourceGrid)
            {
                foreach (Transform child in resourceGrid.transform)
                {
                    Destroy(child.gameObject);
                }
            }
            
            _resourceRows = new List<ResourceRowOutput>();

        }

        private void UpdateResourceQuantities(NaturalResource naturalResource)
        {
            foreach (var outputRow in _resourceRows)
            {
                var resType = outputRow.ResourceType;
                var resValue = naturalResource.GetResource().Quantity;
                outputRow.UpdateQuantity(resValue);
            }
        }

        public void UpdateView(NaturalResource naturalResource)
        {
            //_slider.value = naturalResource.GetStamina();
            UpdateResourceQuantities(naturalResource);
        }
    }
}