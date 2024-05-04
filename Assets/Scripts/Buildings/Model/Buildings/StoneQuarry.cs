namespace MyRTSGame.Model
{
    public class StoneQuarry : ResourceBuilding
    {
        //Constructor
        public StoneQuarry()
        {
            BuildingType = BuildingType.StoneQuarry;
        }

        protected override void Start()
        {
            base.Start();

            OutputTypesWhenCompleted = new[] { ResourceType.Stone };
        }

        protected override void StartResourceCreationCoroutine()
        {
            StartCoroutine(CreateResource( 15, ResourceType.Stone));
        }
    }
}