using Application;
using UnityEngine;

namespace ResourceSystem.View
{
    public class NaturalResourceView: MonoBehaviour
    {
        [SerializeField] public ResourceService resourceService;
        private void Start()
        {
            ServiceInjector.Instance.InjectNaturalResourceViewDependencies(this);
        }
    }
}
