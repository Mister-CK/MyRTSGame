using UnityEngine;
using MyRTSGame.Model;
using System.Linq;
using TMPro;

public class JobListDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI jobListText; 
    private JobQueue _jobQueue;
    void Awake()
    {
        _jobQueue = JobQueue.GetInstance();
    }
    void Update()
    {
        var jobs = _jobQueue.GetJobs();

        jobListText.text = string.Join("\n", jobs.Select(job => job.ResourceType + " " + job.Destination.BuildingType));
    }
}