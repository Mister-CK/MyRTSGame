using Enums;
using Interface;

namespace Terrains.Model.TerrainStates
{
    public class FoundationState: ITerrainState
    {
        private readonly TerrainType _terrainType;
        private readonly TerrainManager _terrainManager = TerrainManager.Instance;

        public FoundationState(TerrainType terrainType)
        {
            _terrainType = terrainType;
        }

        public void SetObject(Terrain terrain)
        {
            var foundation = _terrainManager.FoundationObjects[_terrainType];
            terrain.SetObject(foundation);
        }
    }
}