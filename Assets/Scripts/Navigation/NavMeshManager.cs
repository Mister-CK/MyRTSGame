using UnityEngine;
using Unity.AI.Navigation;
using System.Collections;

namespace Navigation
{
    public class NavMeshManager : MonoBehaviour
    {
        [SerializeField] private NavMeshSurface navMeshSurface;

        public static bool IsNavMeshReady { get; private set; } = false;

        private void Awake()
        {
            IsNavMeshReady = false;
        }

        private IEnumerator Start()
        {
            if (navMeshSurface.navMeshData == null)
            {
                navMeshSurface.BuildNavMesh();
                yield return null;
            }

            IsNavMeshReady = true;
        }

        public void UpdateNavMesh(Bounds boundsToUpdate, CollectObjects originalSetting)
        {
            StartCoroutine(UpdateNavMeshRoutine(boundsToUpdate, originalSetting));
        }

        private IEnumerator UpdateNavMeshRoutine(Bounds boundsToUpdate, CollectObjects originalSetting)
        {
            IsNavMeshReady = false; // Temporarily block unit pathfinding

            // 1. Apply bounds settings
            navMeshSurface.center = boundsToUpdate.center;
            navMeshSurface.size = boundsToUpdate.size;

            // 2. Perform the asynchronous update
            AsyncOperation operation = navMeshSurface.UpdateNavMesh(navMeshSurface.navMeshData);

            // 3. Wait for the update to complete
            yield return operation;

            // 4. Restore and signal readiness
            navMeshSurface.collectObjects = originalSetting;
            IsNavMeshReady = true;
        }
    }
}

