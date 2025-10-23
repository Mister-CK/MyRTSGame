using Enums;
using MyRTSGame.Model;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ResourceRowOutput : MonoBehaviour
{
        public ResourceType ResourceType { get; set; }

        [SerializeField] public TextMeshProUGUI resourceTypeText;
        [SerializeField] public TextMeshProUGUI quantity;
        [SerializeField] public TextMeshProUGUI inOutGoingJobs;
        
        public void UpdateQuantity(int newQuantity)
        {
                quantity.text = newQuantity.ToString();
        }
        
        public void UpdateInOutGoingJobs(int newQuantity)
        {
                inOutGoingJobs.text = newQuantity.ToString();
        }
}