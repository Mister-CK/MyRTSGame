using Buildings.Model;
using Interface;
using Domain.Model;
using Terrains.Model;
using Domain.Units.Component;

namespace Domain.Units.JobExecutors
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
