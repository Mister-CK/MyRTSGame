using Unity.VisualScripting;

namespace MyRTSGame.Model
{
    public class Job
    {
        private bool _inProgress;

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