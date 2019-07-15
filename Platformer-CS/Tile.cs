

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Platformer_CS
{
    public class Tile
    {
        public Texture2D texture;
        public char name;

        public int width;
        public int height;
        public Vector2 position;

        public Rectangle boundingBox;


        public Tile(Vector2 position, int width, int height, char name)
        {
            this.position = position;
            this.width = width;
            this.height = height;
            this.name = name;

            this.boundingBox = new Rectangle((int)position.X, (int)position.Y, width, height);
        }
    }
}