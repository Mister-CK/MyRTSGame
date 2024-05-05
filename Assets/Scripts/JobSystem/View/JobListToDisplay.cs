using System.Linq;
using TMPro;
using UnityEngine;

namespace MyRTSGame.Model
{
    public class JobListDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI jobListText;
        [SerializeField] private VillagerJobQueue villagerJobQueue;
        

        private void Update()
        {
            var jobs = villagerJobQueue.GetJobs();
            jobs.Select(job => job.ResourceType + " " + 
                               (job.Origin is Building originBuilding ? originBuilding.GetBuildingType() : "Not a building") + " " + 
                               (job.Destination is Building destinationBuilding ? destinationBuilding.GetBuildingType() : "Not a building"));
        }
    }
}