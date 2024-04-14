namespace MyRTSGame.Model
{
    public class GoldMine : ResourceBuilding
    {
        //Constructor
        public GoldMine()
        {
            BuildingType = BuildingType.GoldMine;
        }

        protected override void Start()
        {
            base.Start();

            OutputTypesWhenCompleted = new[] { ResourceType.GoldOre };

        }

        public override void StartResourceCreationCoroutine()
        {
            StartCoroutine(CreateResource(15, ResourceType.GoldOre));
        }
    }
}