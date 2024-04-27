namespace MyRTSGame.Model
{
    public class Villager : Unit
    {
        private bool _hasResource;

        public Villager()
        {
            UnitType = UnitType.Villager;
        }

        protected override void ExecuteJob()
        {
            base.ExecuteJob();
            if (CurrentJob is not VillagerJob villagerJob) return;
            
            if (!_hasResource)
            {
                TakeResource(villagerJob.Origin, villagerJob.ResourceType);
                Destination = villagerJob.Destination;
                Agent.SetDestination(Destination.GetPosition());
                return;
            }
            DeliverResource(villagerJob.Destination, villagerJob.ResourceType);
            HasDestination = false;
            
        }

        private void TakeResource(IDestination destination, ResourceType resourceType)
        {
            _hasResource = true;
            unitController.RemoveResourceFromDestination(destination, resourceType, 1);
        }

        private void DeliverResource(IDestination destination, ResourceType resourceType)
        {
            _hasResource = false;
            unitController.AddResourceToDestination(destination, resourceType, 1);
        }

        public void UnAssignVillagerJob(DestinationType destinationType)
        {
            if (destinationType == DestinationType.Origin && _hasResource)
            {
                return;
            }

            HasDestination = false;
            
            Agent.SetDestination(Agent.transform.position);
            CurrentJob = null;
            _hasResource = false;
            Destination = null;
        }
    }
}
