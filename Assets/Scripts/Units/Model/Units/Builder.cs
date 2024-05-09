using UnityEngine;

namespace MyRTSGame.Model
{
    public class Builder : Unit
    {
        public Builder()
        {
            UnitType = UnitType.Builder;
        }
        
        protected override void ExecuteJob()
        {
            base.ExecuteJob();
            if (CurrentJob is not BuilderJob) return;
            if (Destination is Building building)
            {
                building.SetState(new CompletedState(building.GetBuildingType()));
            }
            if (Destination is Terrains.Model.Terrain terrain)
            {
                terrain.SetState(new Terrains.Model.TerrainStates.CompletedState(terrain.GetTerrainType()));
            }
            unitController.CompleteJob(CurrentJob);
            HasDestination = false;
            CurrentJob = null;
            Destination = null;
        }
    }
}