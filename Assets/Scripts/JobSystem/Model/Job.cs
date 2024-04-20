using Unity.VisualScripting;

namespace MyRTSGame.Model
{
    public class Job
    {
        private bool _inProgress;
        public Building Destination { get; set; }
        public Unit Unit;

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