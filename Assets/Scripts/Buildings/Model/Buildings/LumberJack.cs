namespace MyRTSGame.Model
{
    public class Lumberjack : ResourceBuilding
    {
        //Constructor
        public Lumberjack()
        {
            BuildingType = BuildingType.LumberJack;
        }

        protected override void Start()
        {
            base.Start();

            OutputTypesWhenCompleted = new[] { ResourceType.Lumber };
            OccupantType = UnitType.LumberJack;
        }
        
        public override void StartResourceCreationCoroutine()
        {
            // StartCoroutine(CreateResource(5, ResourceType.Lumber));
        }
    }
}