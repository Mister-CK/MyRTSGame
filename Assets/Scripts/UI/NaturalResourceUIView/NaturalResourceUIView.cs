using System.Collections.Generic;
using MyRTSGame.Model.ResourceSystem.Model;
using MyRTSGame.Model.ResourceSystem.Model.ResourceStates;
using TMPro;
using UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace UI.NaturalResourceUIView
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
        NaturalResource _naturalResource;
        
        private void InitGrowthSlider(GrowingState growingState)
        {
            var bar = Instantiate(statusBarPrefab, statusBars.transform);
            _slider = bar.GetComponent<Slider>();
            _slider.value = growingState.GetPercentageGrown();
        }

        public void Update()
        {
            if (_naturalResource.GetState() is GrowingState growingState)
            {
                _slider.value = growingState.GetPercentageGrown();    
            }
        }

        public void ActivateView(NaturalResource naturalResource)
        {
            _naturalResource = naturalResource;
            resourceUIView.gameObject.SetActive(true);
            naturalResourceName.text = naturalResource.name;
            if (naturalResource.GetState() is GrowingState growingState)
            {
                InitGrowthSlider(growingState);
            }
            var resourceType = naturalResource.GetResourceType();
            
            Instantiate(resourceGridTitle, resourceGrid.transform);
            Instantiate(columnsPrefab, resourceGrid.transform);

            var resourceRow = Instantiate(resourceRowPrefab, resourceGrid.transform);
            var resourceRowOutput = resourceRow.GetComponent<ResourceRowOutput>();
            resourceRowOutput.ResourceType = resourceType;
            resourceRowOutput.resourceTypeText.text = resourceType.ToString();
            resourceRowOutput.quantity.text = naturalResource.GetInventory()[resourceType].Current.ToString();
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

            _naturalResource = null;
            _resourceRows = new List<ResourceRowOutput>();

        }

        private void UpdateResourceQuantities(NaturalResource naturalResource)
        {
            foreach (var outputRow in _resourceRows)
            {
                var resType = outputRow.ResourceType;
                var resValue = naturalResource.GetInventory()[outputRow.ResourceType].Current;
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