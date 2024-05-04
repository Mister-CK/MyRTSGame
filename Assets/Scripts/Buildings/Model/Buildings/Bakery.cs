namespace MyRTSGame.Model
{
    public class Bakery : ProductionBuilding
    {
        //Constructor
        public Bakery()
        {
            BuildingType = BuildingType.Bakery;
        }

        protected override void Start()
        {
            base.Start();

            InputTypesWhenCompleted = new[] { ResourceType.Flour };
            OutputTypesWhenCompleted = new[] { ResourceType.Bread };
            HasInput = true;
        }

        protected override void StartResourceCreationCoroutine()
        {
            Resource[] input = { new() { ResourceType = ResourceType.Flour, Quantity = 1 } };
            Resource[] output = { new() { ResourceType = ResourceType.Bread, Quantity = 1 } };
            StartCoroutine(CreateOutputFromInput(10, input, output));
        }
    }
}