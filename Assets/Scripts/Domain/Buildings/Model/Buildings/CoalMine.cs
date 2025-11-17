using Buildings.Model.BuildingGroups;
using Enums;

namespace MyRTSGame.Model
{
    public class CoalMine : ResourceBuilding
    {
        //Constructor
        public CoalMine()
        {
            BuildingType = BuildingType.CoalMine;
        }

        protected override void Start()
        {
            base.Start();

            OutputTypesWhenCompleted = new[] { ResourceType.Coal };
        }

        protected override void StartResourceCreationCoroutine()
        {
            StartCoroutine(CreateResource(15, ResourceType.Coal));
        }
    }
}