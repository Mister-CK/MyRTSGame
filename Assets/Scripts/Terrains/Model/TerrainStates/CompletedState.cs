namespace MyRTSGame.Model.Terrains.Model.TerrainStates
{
    public class CompletedState: ITerrainState
    {
        private readonly TerrainType _terrainType;
        private readonly TerrainManager _terrainManager = TerrainManager.Instance;

        public CompletedState(TerrainType terrainType)
        {
            _terrainType = terrainType;
        }
        public void SetObject(Terrain terrain)
        {
            var completedObject = _terrainManager.CompletedObjects[_terrainType];
            terrain.SetObject(completedObject);
        }
    }
}