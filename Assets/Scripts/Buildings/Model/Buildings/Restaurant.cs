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

        public override void AddResource(ResourceType resourceType, int quantity)
        {
            base.AddResource(resourceType, quantity);
            if (resourceType is not ResourceType.Bread and not ResourceType.Fish and not ResourceType.Sausage and not ResourceType.Wine)
            {
                return;
            }
            buildingController.CreateJobNeededEvent(JobType.ConsumptionJob, this, null, resourceType);
        }
    }
}