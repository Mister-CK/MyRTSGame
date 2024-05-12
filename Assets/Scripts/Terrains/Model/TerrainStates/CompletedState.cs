using MyRTSGame.Model.Terrains.Model.Terrains;

namespace MyRTSGame.Model.Terrains.Model.TerrainStates
{
    public class CompletedState: ITerrainState
    {
        private readonly Terrain _terrain;
        private readonly TerrainManager _terrainManager = TerrainManager.Instance;

        public CompletedState(Terrain terrain)
        {
            _terrain = terrain;
        }
        public void SetObject(Terrain terrain)
        {
            var completedObject = _terrainManager.CompletedObjects[_terrain.GetTerrainType()];
            terrain.SetObject(completedObject);
        }
    }
}