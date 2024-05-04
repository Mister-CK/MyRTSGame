namespace MyRTSGame.Model
{
    public class Butcher : ProductionBuilding
    {
        //Constructor
        public Butcher()
        {
            BuildingType = BuildingType.Butcher;
        }

        protected override void Start()
        {            
            base.Start();
            
            InputTypesWhenCompleted = new[] { ResourceType.Pork };
            OutputTypesWhenCompleted = new[] { ResourceType.Sausage };
            HasInput = true;
        }
        
        protected override void StartResourceCreationCoroutine()
        {
            Resource[] input = { new() { ResourceType = ResourceType.Pork, Quantity = 1 } };
            Resource[] output = { new() { ResourceType = ResourceType.Sausage, Quantity = 1 } };
            StartCoroutine(CreateOutputFromInput(10, input, output));
        }
    }
}