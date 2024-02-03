using System.Linq;
using TMPro;
using UnityEngine;

namespace MyRTSGame.Model
{
    public class JobListDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI jobListText;
        private JobQueue _jobQueue;

        private void Start()
        {
            _jobQueue = JobQueue.GetInstance();
        }

        private void Update()
        {
            var jobs = _jobQueue.GetJobs();

            jobListText.text = string.Join("\n",
                jobs.Select(job => job.ResourceType + " " + job.Origin.BuildingType+ " " + job.Destination.BuildingType));
        }
    }
}