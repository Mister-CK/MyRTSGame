using Buildings.Model;
using Interface;
using MyRTSGame.Model;
using Terrains.Model;
using Units.Model.Component;

namespace Units.Model.JobExecutors
{
    public class BuilderJobExecutor: IJobExecutor
    {
        public void Execute(UnitComponent unitComponent, Job job)
        {
            var builderComponent = (BuilderComponent)unitComponent;
            
            if (builderComponent.Data.Destination is Building building)
            {
                builderComponent.StartCoroutine(builderComponent.Build(building));
            }
            if (builderComponent.Data.Destination is Terrain terrain)
            {
                builderComponent.StartCoroutine(builderComponent.Build(terrain));
            }
        }
    }
}
