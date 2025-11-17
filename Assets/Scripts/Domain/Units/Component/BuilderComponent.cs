using Buildings.Model;
using Enums;
using System.Collections;
using UnityEngine;
using Terrain = Terrains.Model.Terrain;
using Interface;
using MyRTSGame.Model;
using System;
using Units.Model.Data;
using Units.Model.JobExecutors;

namespace Units.Model.Component
{
    public class BuilderComponent : UnitComponent
    {
        protected override JobType DefaultJobType => JobType.BuilderJob;

        static BuilderComponent()
        {
            JobExecutorsMap.Add(typeof(BuilderJob), new BuilderJobExecutor());

        }
        protected override UnitData CreateUnitData()
        {
            return new BuilderData();
        }

        public IEnumerator Build(IBuildable buildable)
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
            OnJobCompleted?.Invoke(Data.CurrentJob);
            Data.SetDestination(null);
            Data.SetHasDestination(false);
            Data.SetCurrentJob(null);

        }
    }
}