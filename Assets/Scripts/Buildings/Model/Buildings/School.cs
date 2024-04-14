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
            trainableUnits =  new List<UnitType>() {UnitType.Villager, UnitType.Builder};
            TrainingJobs = GetTrainingJobsForUnitTypes(trainableUnits);
            HasInput = true;
        }
        
        public override void StartResourceCreationCoroutine()
        {
            StartCoroutine(TrainUnitFromQueue(5));
        }
    }
}