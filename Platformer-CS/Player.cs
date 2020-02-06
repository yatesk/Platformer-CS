using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Platformer_CS
{
    public class Player
    {
        public Vector2 position;
        public Vector2 velocity;
        public Texture2D image;
        public int width;
        public int height;
        public bool onPlatform = true;

        public Player(int x, int y, int width, int height)
        {
            position = new Vector2(x, y);
            velocity = new Vector2(0, 0);

            this.width = width;
            this.height = height;
        }

        public void Left()
        {
            velocity.X = -5;
        }

        public void Right()
        {
            velocity.X = 5;
        }

        public void Stop()
        {
            velocity.X = 0;
        }

        public void Jump()
        {
            if (onPlatform)
                velocity.Y = -8;  
        }

        public bool isOnPlatform(Level level)
        {
            // check to see if player is on the ground
            Rectangle playerBoundingBox = new Rectangle((int)position.X, (int)position.Y + 1, width, height);

            for (int i = 0; i < level.tiles.Count; i++)
            {
                Rectangle tileBoundingBox = new Rectangle((int)level.tiles[i].position.X, (int)level.tiles[i].position.Y, level.tiles[i].width, level.tiles[i].height);

                Rectangle intersectArea = Rectangle.Intersect(playerBoundingBox, tileBoundingBox);

                if (intersectArea.Height == 1)
                    return true;
            }
            return false;
        }

        public void Gravity()
        {
            if (!onPlatform && velocity.Y <= 8)
                velocity.Y += .25f;      
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, position);
        }
    }
}
