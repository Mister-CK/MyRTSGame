using Enums;
using Interface;

namespace MyRTSGame.Model
{
    public class Villager : Unit
    {
        private bool _hasResource;
        
        public Villager()
        {
            UnitType = UnitType.Villager;
        }
        
        public bool GetHasResource()
        {
            return _hasResource;
        }
        
        public void SetHasResource(bool hasResource)
        {
            _hasResource = hasResource;
        }


        protected override void ExecuteJob()
        {
            base.ExecuteJob();
            if (CurrentJob is not VillagerJob villagerJob) return;
            
            if (!_hasResource)
            {
                TakeResource(villagerJob.Origin, villagerJob.ResourceType);
                Destination = villagerJob.Destination;
                HasJobToExecute = true;
                Agent.SetDestination(Destination.GetPosition());
                return;
            }
            DeliverResource(villagerJob.Destination, villagerJob.ResourceType);
            unitService.CompleteJob(CurrentJob);
            HasDestination = false;
            CurrentJob = null;
            Destination = null;
        }

        private void TakeResource(IDestination destination, ResourceType resourceType)
        {
            _hasResource = true;
            unitService.RemoveResourceFromDestination(destination, resourceType, 1);
        }

        private void DeliverResource(IDestination destination, ResourceType resourceType)
        {
            _hasResource = false;
            unitService.AddResourceToDestination(destination, resourceType, 1);
        }
    }
}
