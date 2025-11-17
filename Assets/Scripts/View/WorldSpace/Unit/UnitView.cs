using Application;
using Application.Services;
using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Domain.Model
{
    public class UnitView : MonoBehaviour
    {
        [FormerlySerializedAs("unitController")] public UnitService unitService;
        
        private void Start()
        {
            ServiceInjector.Instance.InjectUnitDependencies(this);
        }

        protected virtual void OnMouseDown()
        {
            throw new NotImplementedException();
        }
    }
}