using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Platformer_CS
{
    public class Tile
    {
        public Texture2D texture;
        public TileType type;

        public int width;
        public int height;
        public Vector2 position;
        
        public enum TileType {Brick, Floor1, Question1, Question2, Pipe, Stair };

        public Tile(Vector2 position, int width, int height, TileType type)
        {
            this.position = position;
            this.width = width;
            this.height = height;
            this.type = type;
        }
    }
}