using Enums;
using Interface;
using TMPro;
using UnityEngine;

namespace UI.Components
{
        public class ResourceRowInput : MonoBehaviour, IResourceRow
        {
                public ResourceType ResourceType { get; set; }

                [SerializeField] public TextMeshProUGUI resourceTypeText;
                [SerializeField] public TextMeshProUGUI quantity;
                [SerializeField] public TextMeshProUGUI inIncomingJobs;

                public void UpdateQuantity(int newQuantity)
                {
                        quantity.text = newQuantity.ToString();
                }

                public void UpdateJobs(int newQuantity)
                {
                        inIncomingJobs.text = newQuantity.ToString();
                }
        }
}