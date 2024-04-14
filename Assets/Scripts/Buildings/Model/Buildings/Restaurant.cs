namespace MyRTSGame.Model
{
    public class Restaurant : ConsumptionBuilding
    {
        //Constructor
        public Restaurant()
        {
            BuildingType = BuildingType.Restaurant;
        }

        protected override void Start()
        {            
            base.Start();
            
            InputTypesWhenCompleted = new[] { ResourceType.Bread, ResourceType.Fish, ResourceType.Sausage, ResourceType.Wine};
            HasInput = true;
        }
    }
}