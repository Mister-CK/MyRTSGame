using System.Collections;
using UnityEngine;

namespace MyRTSGame.Model.ResourceSystem.Model.ResourceStates
{
    public class GrowingState: IResourceState
    {
        private float _percentageGrown;
        private NaturalResource _naturalResource;
        private MonoBehaviour _monoBehaviour;

        public GrowingState(NaturalResource naturalResource, MonoBehaviour monoBehaviour)
        {
            _naturalResource = naturalResource;
            _monoBehaviour = monoBehaviour;
            _monoBehaviour.StartCoroutine(HandleGrowth());
        }

        private IEnumerator HandleGrowth()
        {
            while (_percentageGrown < 100)
            {
                _percentageGrown += .2f; 
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