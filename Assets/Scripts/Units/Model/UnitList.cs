using System.Collections.Generic;
using UnityEngine;

namespace MyRTSGame.Model
{
    public class UnitList : MonoBehaviour
    {
        private List<Unit> _units;
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

            _units = new List<Unit>(FindObjectsOfType<Unit>());
        }
        
        public List<Unit> GetUnits()
        {
            return _units;
        }

        public void AddUnit(Unit newUnit)
        {
            _units.Add(newUnit);
        }
    }
}