namespace MyRTSGame.Model
{
    public class ConsumptionJob: Job
    {
        public ResourceType ResourceType { get; set; }
        public Unit Unit { get; set; }
    }
}