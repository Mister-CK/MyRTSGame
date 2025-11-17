using Interface;
using Domain.Units.Component;

namespace Domain.Model
{
    public class Job
    {
        private bool _inProgress;
        public IDestination Destination { get; set; }
        public UnitComponent Unit;

        public bool IsInProgress()
        {
            return _inProgress;
        }
        
        public void SetInProgress(bool inProgress)
        {
            _inProgress = inProgress;
        }
    }
}