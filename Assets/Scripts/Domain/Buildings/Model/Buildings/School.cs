using Buildings.Model.BuildingGroups;
using Enums;
using System.Collections.Generic;

namespace MyRTSGame.Model
{
    public class School : TrainingBuilding
    {
        //Constructor
        public School()
        {
            BuildingType = BuildingType.School;
        }

        protected override void Start()
        {            
            base.Start();
            
            InputTypesWhenCompleted = new[] { ResourceType.Gold };
            trainableUnits =  new List<UnitType>() {UnitType.Villager, UnitType.Builder, UnitType.StoneMiner, UnitType.LumberJack, UnitType.Farmer};
            TrainingJobs = GetTrainingJobsForUnitTypes(trainableUnits);
        }
        
        protected override void StartResourceCreationCoroutine()
        {
            StartCoroutine(TrainUnitFromQueue(5));
        }
    }
}