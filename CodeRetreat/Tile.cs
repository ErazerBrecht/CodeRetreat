namespace CodeRetreat
{
    public class Tile
    {
        public TileType TileType { get; }

        public Tile(TileType tileType)
        {
            TileType = tileType;
        }
    }

    public enum TileType
    {
        Wall,
        EmptySpace,
        Teleport1
    }
}