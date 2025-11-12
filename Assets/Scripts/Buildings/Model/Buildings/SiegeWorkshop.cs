using Buildings.Model.BuildingGroups;
using Enums;
using System.Collections.Generic;

namespace MyRTSGame.Model
{
    public class SiegeWorkshop : TrainingBuilding
    {
        //Constructor
        public SiegeWorkshop()
        {
            BuildingType = BuildingType.SiegeWorkshop;
        }

        protected override void Start()
        {            
            base.Start();
            
            InputTypesWhenCompleted = new[] {ResourceType.Wood, ResourceType.Iron, };
            //TODO: should have siege weapons as trainingJobs, instead of villagers and builders
            trainableUnits =  new List<UnitType>() {UnitType.Villager, UnitType.Builder};
            TrainingJobs = GetTrainingJobsForUnitTypes(trainableUnits);
        }
        
        protected override void StartResourceCreationCoroutine()
        {
            StartCoroutine(TrainUnitFromQueue(5));
        }
    }
}