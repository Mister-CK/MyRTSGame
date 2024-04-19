using System;
using MyRTSGame.Model;

public class ConsumptionJobEventArgs: EventArgs, IGameEventArgs
{
    public ConsumptionJob ConsumptionJob { get; }

    public ConsumptionJobEventArgs(ConsumptionJob consumptionJob)
    {
        ConsumptionJob = consumptionJob;
    }
}
