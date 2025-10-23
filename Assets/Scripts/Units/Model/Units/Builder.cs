using Buildings.Model;
using System.Collections;
using UnityEngine;
using Terrain = Terrains.Model.Terrain;
using Enums;
using Interface;

namespace MyRTSGame.Model
{
    public class Builder : Unit
    {
        public Builder()
        {
            UnitType = UnitType.Builder;
        }

        private IEnumerator Build(IBuildable buildable)
        {
            if (buildable is Building building)
            {
                while (building.GetState() is ConstructionState constructionState)
                {
                    yield return new WaitForSecondsRealtime(.1f);
                    constructionState
                        .IncreasePercentageCompleted(building.GetBuildRate());
                }
            }

            if (buildable is Terrain terrain)
            {
                terrain.SetState(new Terrains.Model.TerrainStates.ConstructionState(terrain));
                while (terrain.GetState() is Terrains.Model.TerrainStates.ConstructionState constructionState)
                {
                    yield return new WaitForSecondsRealtime(.1f);
                    constructionState
                        .IncreasePercentageCompleted(terrain.GetBuildRate());
                }
            }
            unitService.CompleteJob(CurrentJob);
            Destination = null;
            HasDestination = false;
            CurrentJob = null;

        }
        
        protected override void ExecuteJob()
        {
            base.ExecuteJob();

            if (CurrentJob is not BuilderJob) return;
            if (Destination is Building building)
            {
                StartCoroutine(Build(building));
            }
            if (Destination is Terrains.Model.Terrain terrain)
            {
                StartCoroutine(Build(terrain));
            }

        }
    }
}