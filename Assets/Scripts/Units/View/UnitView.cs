using Application;
using Application.Services;
using UnityEngine;
using UnityEngine.Serialization;

namespace MyRTSGame.Model
{
    public class UnitView : MonoBehaviour
    {
        [FormerlySerializedAs("unitController")] public UnitService unitService;
        
        private void Start()
        {
            ServiceInjector.Instance.InjectUnitDependencies(this);
        }
    }
}