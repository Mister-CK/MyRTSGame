using Buildings.Model;
using System.Collections;
using UnityEngine;
using Terrain = Terrains.Model.Terrain;
using Interface;
using MyRTSGame.Model;
using Units.Model.Data;

namespace Units.Model.Component
{
    public class BuilderComponent : UnitComponent
    {
        protected override UnitData CreateUnitData()
        {
            return new BuilderData();
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
            unitService.CompleteJob(Data.CurrentJob);
            Data.SetDestination(null);
            Data.SetHasDestination(false);
            Data.SetCurrentJob(null);

        }
        
        protected override void ExecuteJob()
        {
            base.ExecuteJob();

            if (Data.CurrentJob is not BuilderJob) return;
            if (Data.Destination is Building building)
            {
                StartCoroutine(Build(building));
            }
            if (Data.Destination is Terrain terrain)
            {
                StartCoroutine(Build(terrain));
            }

        }
    }
}