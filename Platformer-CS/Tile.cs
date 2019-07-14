

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Platformer_CS
{
    public class Tile
    {
        public Texture2D texture;
        public string name;

        public int width;
        public int height;
        public int x;
        public int y;


        public Tile(int x, int y, int width, int height, string name)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.name = name;
              
        }
            
    }
}
