using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Platformer_CS
{
    public class Player
    {
        public Vector2 position;
        public Vector2 velocity;
        public Texture2D image;

        public Rectangle boundingBox;

        public Player(int x, int y)
        {
            position = new Vector2(x, y);
            velocity = new Vector2(0, 0);

            boundingBox = new Rectangle(x, y, 32, 64);
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
            // jump
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, position);
        }
    }

}
