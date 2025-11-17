using Interface;
using Domain.Model;
using Domain.Units.Component;

namespace Domain.Units.JobExecutors
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
