using System.Collections.Generic;
using Domain.Units.Component;
using UnityEngine;

namespace Domain.Units
{
    public class UnitList : MonoBehaviour
    {
        private List<UnitComponent> _units;
        public static UnitList Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            _units = new List<UnitComponent>(FindObjectsOfType<UnitComponent>());
        }
        
        public List<UnitComponent> GetUnits()
        {
            return _units;
        }

        public void AddUnit(UnitComponent newUnit)
        {
            _units.Add(newUnit);
        }
    }
}