using System;
using MyRTSGame.Model;
using UnityEngine;

public class VillagerChild : MonoBehaviour
{
    private void OnMouseDown()
    {
        var villager = GetComponentInParent<Villager>();
        if (villager != null)
        {
            villager.HandleClick();
        }
    }
}