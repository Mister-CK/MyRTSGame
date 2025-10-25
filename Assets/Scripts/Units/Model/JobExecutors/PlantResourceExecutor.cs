using Buildings.Model;
using Interface;
using MyRTSGame.Model;
using Units.Model.Component;

namespace Units.Model.JobExecutors
{
    public class PlantResourceExecutor : IJobExecutor
    {
        public void Execute(UnitComponent unit, Job job)
        {
            var rc = (ResourceCollectorComponent)unit;

            if (unit.Data.Destination is Building destinationBuilding && destinationBuilding == rc.CollectorData.Building) return;
            unit.unitService.CreatePlantResourceEvent(job);
            unit.unitService.CompleteJob(job); 
            
            unit.Data.SetDestination(rc.CollectorData.Building);
            unit.Agent.SetDestination(unit.Data.Destination.GetPosition());
            unit.Data.SetHasJobToExecute(true); 
            unit.Data.SetCurrentJob(null); 
        }
    }
}
