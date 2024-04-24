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

            jobListText.text = string.Join("\n",
                jobs.Select(job => job.ResourceType + " " + job.Origin.GetBuildingType()+ " " + job.Destination.GetBuildingType()));
        }
    }
}