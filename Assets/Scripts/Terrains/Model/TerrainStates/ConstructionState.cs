namespace MyRTSGame.Model.Terrains.Model.TerrainStates
{
    public class ConstructionState: ITerrainState
    {
        private readonly TerrainType _terrainType;
        private readonly TerrainManager _terrainManager = TerrainManager.Instance;

        public ConstructionState(TerrainType terrainType)
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