using UnityEngine;

namespace MyRTSGame.Model
{
    public class Lumberjack : ResourceBuilding
    {
        private const float FreeSpaceAroundPoint = 2f;

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
        
        protected override void StartResourceCreationCoroutine()
        {
            // StartCoroutine(CreateResource(5, ResourceType.Lumber));
        }
        
        private Vector3 GetRandomPointToPlantTree()
        {
            var randomPoint = new Vector3(0, 0, 0);
            while (Physics.CheckSphere(randomPoint, FreeSpaceAroundPoint))
            {
                var randomX = Random.Range(-MaxDistanceFromBuilding, MaxDistanceFromBuilding);
                var randomZ = Random.Range(-MaxDistanceFromBuilding, MaxDistanceFromBuilding);
                randomPoint = new Vector3(randomX, 0, randomZ) + transform.position;

            }

            return randomPoint;
        }
    }
}