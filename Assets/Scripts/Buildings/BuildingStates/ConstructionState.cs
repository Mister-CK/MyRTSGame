using MyRTSGame.Model;
using MyRTSGame.Interface;
using UnityEngine;


public class ConstructionState : IBuildingState
{
    public GameObject completedObject; // The completed GameObject

    public void OnClick(Building building)
    {
        // Handle click when in CompletedState
    }

    public void SetObject(Building building)
    {
        // Set the GameObject to a completed building
        building.SetObject(completedObject);
    }
}