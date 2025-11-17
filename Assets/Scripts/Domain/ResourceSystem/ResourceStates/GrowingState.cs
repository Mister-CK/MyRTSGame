using Interface;
using System.Collections;
using UnityEngine;

namespace Domain.Model.ResourceSystem.Model.ResourceStates
{
    public class GrowingState: IResourceState
    {
        private float _percentageGrown;
        private readonly NaturalResource _naturalResource;

        public GrowingState(NaturalResource naturalResource, MonoBehaviour monoBehaviour, float growthRate)
        {
            _naturalResource = naturalResource;
            monoBehaviour.StartCoroutine(HandleGrowth(growthRate));
        }

        private IEnumerator HandleGrowth(float growthRate)
        {
            while (_percentageGrown < 100)
            {
                _percentageGrown += growthRate; 
                yield return new WaitForSeconds(.1f);

                if (_percentageGrown >= 100)
                {
                    _naturalResource.SetState(new CompletedState());
                }
            }
        }

        public float GetPercentageGrown()
        {
            return _percentageGrown;
        }
    }
}