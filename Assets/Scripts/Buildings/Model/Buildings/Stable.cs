namespace MyRTSGame.Model
{
    public class Stable : ProductionBuilding
    {
        //Constructor
        public Stable()
        {
            BuildingType = BuildingType.Stable;
        }

        protected override void Start()
        {            
            base.Start();
            
            InputTypesWhenCompleted = new[] { ResourceType.Wheat };
            OutputTypesWhenCompleted = new[] { ResourceType.Horses};
            HasInput = true;
        }
        
        public override void StartResourceCreationCoroutine()
        {
            Resource[] input = { new() { ResourceType = ResourceType.Wheat, Quantity = 1 } };
            Resource[] output = { new() { ResourceType = ResourceType.Horses, Quantity = 1 } };
            StartCoroutine(CreateOutputFromInput(10, input, output));
        }
    }
}