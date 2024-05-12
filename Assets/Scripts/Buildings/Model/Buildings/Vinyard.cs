namespace MyRTSGame.Model
{
    public class Vineyard : ResourceBuilding
    {
        //Constructor
        public Vineyard()
        {
            BuildingType = BuildingType.Vineyard;
        }

        protected override void Start()
        {
            base.Start();

            OutputTypesWhenCompleted = new[] { ResourceType.Wine };
            OccupantType = UnitType.Farmer;

        }

        protected override void StartResourceCreationCoroutine()
        {
            // StartCoroutine(CreateResource( 15, ResourceType.Wine));
        }
    }
}