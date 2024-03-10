using MyRTSGame.Model;
using TMPro;
using UnityEngine;

public class ResourceRowOutput : MonoBehaviour
{
        [SerializeField] public TextMeshProUGUI ResourceType;
        [SerializeField] public TextMeshProUGUI Quantity;
        
        public void UpdateQuantity(int newQuantity)
        {
                Quantity.text = newQuantity.ToString();
        }
}