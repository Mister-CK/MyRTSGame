using Enums;

namespace Terrains.Model.Terrains
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