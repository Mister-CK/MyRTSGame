using Interface;
using UnityEngine;

namespace Domain.LocationDestination
{
    public class LocationDestination : MonoBehaviour, IDestination
    {
        public LocationDestination(Vector3 position)
        {
            transform.position = position;
        }
        public Vector3 GetPosition()
        {
            return transform.position;
        }
    }
}