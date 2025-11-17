using Enums;

namespace Terrains.Model.Terrains
{
    public class Vine : Terrain
    {
        public Vine()
        {
            TerrainType = TerrainType.Vine;
            ResourceType = ResourceType.Wine;
        }
    }
}