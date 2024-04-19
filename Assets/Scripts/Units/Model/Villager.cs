namespace MyRTSGame.Model
{
    public class Villager : Unit
    {
        private Resource _resource = new() { ResourceType = ResourceType.Stone, Quantity = 1 };
        private bool _hasResource;

        public Villager()
        {
            UnitType = UnitType.Villager;
        }

        protected override void ExecuteJob()
        {
            if (CurrentJob is not VillagerJob villagerJob) return;
            
            if (!_hasResource)
            {
                TakeResource(villagerJob.Origin, villagerJob.ResourceType);
                Destination = villagerJob.Destination;
                Agent.SetDestination(Destination.transform.position);
                return;
            }
            DeliverResource(villagerJob.Destination, villagerJob.ResourceType);
            HasDestination = false;
            
        }

        private void TakeResource(Building building, ResourceType resourceType)
        {
            _hasResource = true;
            unitController.RemoveResourceFromBuilding(building, resourceType, 1);
        }

        private void DeliverResource(Building building, ResourceType resourceType)
        {
            _hasResource = false;
            unitController.AddResourceToBuilding(building, resourceType, 1);
        }

        private void RequestNewJob()
        {
            unitController.CreateUnitJobRequest(this);
        }

        public void AcceptNewVillagerJob(VillagerJob villagerJob)
        {
            CurrentJob = villagerJob;
            Destination = villagerJob.Origin;
            _resource.ResourceType = villagerJob.ResourceType;
            Agent.SetDestination(Destination.transform.position);
            HasDestination = true;
        }

        public void UnAssignVillagerJob(DestinationType destinationType)
        {
            if (destinationType == DestinationType.Origin && _hasResource)
            {
                return;
            }

            HasDestination = false;
            CurrentJob = null;
            _hasResource = false;
            Destination = null;
        }

        protected override void SetDestination()
        {
            RequestNewJob();
        }
    }
}
