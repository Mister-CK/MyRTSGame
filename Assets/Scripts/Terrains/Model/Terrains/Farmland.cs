using MyRTSGame.Model.ResourceSystem.Model.NaturalResources;

namespace MyRTSGame.Model.Terrains.Model.Terrains
{
    public class Farmland: Terrain
    {
        public Farmland()
        {
            TerrainType = TerrainType.Farmland;
            ResourceType = ResourceType.Wheat;
        }
    }
}