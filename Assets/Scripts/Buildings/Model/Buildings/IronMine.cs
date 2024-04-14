namespace MyRTSGame.Model
{
    public class IronMine : ResourceBuilding
    {
        //Constructor
        public IronMine()
        {
            BuildingType = BuildingType.IronMine;
        }

        protected override void Start()
        {
            base.Start();

            OutputTypesWhenCompleted = new[] { ResourceType.IronOre };
        }

        public override void StartResourceCreationCoroutine()
        {
            StartCoroutine(CreateResource( 15, ResourceType.IronOre));
        }
    }
}