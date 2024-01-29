using MyRTSGame.Model;
using UnityEngine;

public class VillagerChild : MonoBehaviour
{
    private void OnMouseDown()
    {
        Debug.Log("Child");
        Villager villager = GetComponentInParent<Villager>();
        if (villager != null)
        {
            Debug.Log("hasVil");
            villager.HandleClick();
        }
    }
}