using MyRTSGame.Model;
using Units.Model.Component;

namespace Interface
{
    // Interface for different strategies to execute jobs (implementation of the Strategy Pattern)
    public interface IJobExecutor
    {
        void Execute(UnitComponent unitComponent, Job job);
    }
}
