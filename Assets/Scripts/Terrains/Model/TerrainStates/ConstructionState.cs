using UnityEngine;

namespace MyRTSGame.Model.Terrains.Model.TerrainStates
{
    public class ConstructionState: ITerrainState
    {
        private readonly Terrain _terrain;
        private readonly TerrainManager _terrainManager = TerrainManager.Instance;
        private float _percentageCompleted = 0;
        public ConstructionState(Terrain  terrain)
        {
            _terrain = terrain;
        }

        public void SetObject(Terrain terrain)
        {
            var foundation = _terrainManager.FoundationObjects[_terrain.GetTerrainType()];
            terrain.SetObject(foundation);
        }
        public void IncreasePercentageCompleted(float percentage)
        {
            _percentageCompleted += percentage;
            if (_percentageCompleted >= 100)
            {
                _terrain.SetState(new CompletedState(_terrain));
            }
        }
    }
}