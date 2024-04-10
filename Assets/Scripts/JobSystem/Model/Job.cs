namespace MyRTSGame.Model
{
    public class Job
    {
        private bool InProgress;  
        
        public bool IsInProgress()
        {
            return InProgress;
        }
        
        public void SetInProgress(bool inProgress)
        {
            InProgress = inProgress;
        }
    }
}