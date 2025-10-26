using Buildings.Model;
using Interface;
using MyRTSGame.Model;
using Units.Model.Component;
using UnityEngine;

namespace Units.Model.JobExecutors
{
    public class PlantResourceExecutor : IJobExecutor
    {
        public void Execute(UnitComponent unit, Job job)
        {
            var rc = (ResourceCollectorComponent)unit;
            if (rc.CollectorData.Destination is Building != rc.CollectorData.Building)
            {
                rc.unitService.CreatePlantResourceEvent(job);
                rc.Data.SetDestination(rc.CollectorData.Building);
                rc.Agent.SetDestination(unit.Data.Destination.GetPosition());
                rc.Data.SetHasJobToExecute(true);
                return;
            }
            
            rc.unitService.CompleteJob(job);
            rc.CollectorData.SetHasDestination(false);
            rc.CollectorData.SetCurrentJob(null);
            rc.CollectorData.SetDestination(null);
        }
    }
}
