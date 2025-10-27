using Enums;
using Interface;
using TMPro;
using UnityEngine;

namespace UI.Components
{
        public class ResourceRowOutput : MonoBehaviour, IResourceRow
        {
                public ResourceType ResourceType { get; set; }

                [SerializeField] public TextMeshProUGUI resourceTypeText;
                [SerializeField] public TextMeshProUGUI quantity;
                [SerializeField] public TextMeshProUGUI inOutGoingJobs;

                public void UpdateQuantity(int newQuantity)
                {
                        quantity.text = newQuantity.ToString();
                }

                public void UpdateJobs(int newQuantity)
                {
                        inOutGoingJobs.text = newQuantity.ToString();
                }
        }
}