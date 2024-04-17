namespace MyRTSGame.Model
{
    public class BuilderJob: Job
    {
        public Building Destination { get; set; }
        public Builder Builder { get; set; }
    }
}