using Interface;
using MyRTSGame.Model;
using Units.Model.Component;

namespace Units.Model.JobExecutors
{
    public class ConsumptionExecutor: IJobExecutor
    {
        public void Execute(UnitComponent unitComponent, Job job)
        {
            var consumptionJob = (ConsumptionJob)job;
        
            unitComponent.OnRemoveResourceFromDestination?.Invoke(consumptionJob.Destination, consumptionJob.ResourceType, 1);
        
            unitComponent.Data.ResetJobState();
            unitComponent.Data.ReplenishStamina();
            unitComponent.Data.SetRequestedConsumptionJob(false);
        }
    }
}
