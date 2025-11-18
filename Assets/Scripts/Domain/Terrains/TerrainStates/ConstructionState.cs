using Interface;

namespace Terrains.Model.TerrainStates
{
    public class ConstructionState: ITerrainState
    {
        private readonly Terrain _terrain;
        private float _percentageCompleted = 0;
        public ConstructionState(Terrain  terrain)
        {
            _terrain = terrain;
        }
        
        public void IncreasePercentageCompleted(float percentage)
        {
            _percentageCompleted += percentage;
            if (_percentageCompleted >= 100)
            {
                _terrain.SetState(new CompletedState());
            }
        }
    }
}