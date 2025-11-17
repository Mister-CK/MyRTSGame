using Interface;
using Units.Model.Component;

namespace MyRTSGame.Model
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