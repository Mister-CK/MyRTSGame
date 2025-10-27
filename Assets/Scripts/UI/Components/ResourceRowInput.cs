using Enums;
using TMPro;
using UnityEngine;

namespace UI.Components
{
        public class ResourceRowInput : MonoBehaviour
        {
                public ResourceType ResourceType { get; set; }

                [SerializeField] public TextMeshProUGUI resourceTypeText;
                [SerializeField] public TextMeshProUGUI quantity;
                [SerializeField] public TextMeshProUGUI inIncomingJobs;

                public void UpdateQuantity(int newQuantity)
                {
                        quantity.text = newQuantity.ToString();
                }

                public void UpdateInIncomingJobs(int newQuantity)
                {
                        inIncomingJobs.text = newQuantity.ToString();
                }
        }
}