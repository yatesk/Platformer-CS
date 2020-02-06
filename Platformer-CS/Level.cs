using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;


namespace Platformer_CS
{
    public class Level
    {
        //System.Diagnostics.Debug.WriteLine(timer);

        private Texture2D background_image;

        private Vector2 background_xy1;
        private Vector2 background_xy2;  // max screen width

        // Temporary values so collisions work better.
        private int playerHeight = 62;
        private int playerWidth = 31;

        public Player player;

        private int screen_width = 1200;
        private int screen_height = 800;

        private float timer = 0f;

        public List<Tile> tiles = new List<Tile>();

        private Dictionary<Tile.TileType, Texture2D> tileTextures = new Dictionary<Tile.TileType, Texture2D>();

        private ContentManager content;
        private SpriteFont timerFont;

        public Level(ContentManager Content)
        {
            player = new Player(400, 400, playerWidth, playerHeight);

            background_xy1 = new Vector2(0, 0);
            background_xy2 = new Vector2(screen_width, 0);

            content = Content;

            LoadContent();
            LoadLevel();
        }

        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;


            player.onPlatform = player.isOnPlatform(this);

            player.Gravity();

            if (player.position.X < 0)
                player.position.X = 0;

            Rectangle playerBoundingBox = new Rectangle((int)(player.position.X + player.velocity.X), (int)(player.position.Y), player.width, player.height);

            for (int i = 0; i < tiles.Count; i++)
            {
                Rectangle tileBoundingBox = new Rectangle((int)tiles[i].position.X, (int)tiles[i].position.Y, tiles[i].width, tiles[i].height);

                if (playerBoundingBox.Intersects(tileBoundingBox))
                {
                    if (player.velocity.X > 0)
                    {
                        player.position.X = (int)tiles[i].position.X - player.width;
                        player.velocity.X = 0;
                        break;
                    }
                    else if (player.velocity.X < 0)
                    {
                        player.position.X = (int)tiles[i].position.X + tiles[i].width;
                        player.velocity.X = 0;
                        break;
                    }
                }
            }


            playerBoundingBox = new Rectangle((int)(player.position.X), (int)(player.position.Y + player.velocity.Y), player.width, player.height);

            for (int i = 0; i < tiles.Count; i++)
            {
                Rectangle tileBoundingBox = new Rectangle((int)tiles[i].position.X, (int)tiles[i].position.Y, tiles[i].width, tiles[i].height);

                if (playerBoundingBox.Intersects(tileBoundingBox))
                {
                    if (player.velocity.Y > 0)
                    {
                        player.position.Y = (int)tiles[i].position.Y - player.height;
                        
                        player.velocity.Y = 0;
                        break;
                    }
                    else if (player.velocity.Y < 0)
                    {
                        player.position.Y = (int)tiles[i].position.Y + tiles[i].height;

                        if (tiles[i].type == Tile.TileType.Brick)
                        {
                            tiles.Remove(tiles[i]);
                        }

                        else if (tiles[i].type == Tile.TileType.Question1)
                            tiles[i].type = Tile.TileType.Question2;

                        player.velocity.Y = 0;
                        break;
                    }
                }
            }

            // only move camera if play is in middle of screen and going right
            if (player.velocity.X > 0 && player.position.X >= screen_width / 2)
            {
                MoveTiles();
                background_xy1.X -= player.velocity.X;
                background_xy2.X -= player.velocity.X;

                if (background_xy1.X <= -screen_width)
                    background_xy1.X = screen_width;

                if (background_xy2.X <= -screen_width)
                    background_xy2.X = screen_width;
            }
            else
            {
                player.position.X += player.velocity.X;
            }

            player.position.Y += player.velocity.Y;

            // If player dies, restart level.
            if (player.position.Y > screen_height)
            {
                RestartLevel();
            }
        }

        public void RestartLevel()
        {
            background_xy1 = new Vector2(0, 0);
            background_xy2 = new Vector2(screen_width, 0);
            tiles.Clear();
            timer = 0f;

            LoadLevel();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background_image, background_xy1);
            spriteBatch.Draw(background_image, background_xy2);

            for (int i = 0; i < tiles.Count; i++)
                spriteBatch.Draw(tileTextures[tiles[i].type], tiles[i].position);

            player.Draw(spriteBatch);


            // Draw timer in minutes:seconds format.
            int time = (int)timer;

            if (time % 60 < 10)
            {
                spriteBatch.DrawString(timerFont, (time / 60).ToString() + ":0" + (time % 60).ToString(), new Vector2(0, 0), Color.White);
            }
            else
            {
                spriteBatch.DrawString(timerFont, (time / 60).ToString() + ":" + (time % 60).ToString(), new Vector2(0, 0), Color.White);
            }
        }

        public void LoadLevel()
        {
            String line;
            List<char[]> level = new List<char[]>();
            FileStream fsSource = new FileStream("level1.txt", FileMode.Open, FileAccess.Read);
            using (StreamReader sr = new StreamReader(fsSource))
            {
                while ((line = sr.ReadLine()) != null)
                    level.Add(line.ToCharArray());
            }

            for (int i = 0; i < level.Count; i++)
                for (int j = 0; j < level[i].Length; j++)
                {
                    if (level[i][j] == 'B')
                        tiles.Add(new Tile(new Vector2(64 * j, 64 * i), 64, 64, Tile.TileType.Brick));

                    if (level[i][j] == 'F')
                        tiles.Add(new Tile(new Vector2(64 * j, 64 * i), 64, 64, Tile.TileType.Floor1));

                    if (level[i][j] == 'Q')
                        tiles.Add(new Tile(new Vector2(64 * j, 64 * i), 64, 64, Tile.TileType.Question1));

                    if (level[i][j] == 'P')
                        tiles.Add(new Tile(new Vector2(64 * j, 64 * i), 128, 64, Tile.TileType.Pipe));

                    if (level[i][j] == 'S')
                        tiles.Add(new Tile(new Vector2(64 * j, 64 * i), 64, 64, Tile.TileType.Stair));

                    if (level[i][j] == 'Z')
                    {
                        player.position.X = 64 * j;
                        player.position.Y = 64 * i;
                    }
                }
        }

        public void LoadContent()
        {
            background_image = content.Load<Texture2D>("level2");
            player.image = content.Load<Texture2D>("player4");

            tileTextures.Add(Tile.TileType.Brick, content.Load<Texture2D>("brick"));
            tileTextures.Add(Tile.TileType.Floor1, content.Load<Texture2D>("floor1"));
            tileTextures.Add(Tile.TileType.Question1, content.Load<Texture2D>("question1"));
            tileTextures.Add(Tile.TileType.Question2, content.Load<Texture2D>("question2"));
            tileTextures.Add(Tile.TileType.Pipe, content.Load<Texture2D>("pipe"));
            tileTextures.Add(Tile.TileType.Stair, content.Load<Texture2D>("stair"));

            timerFont = content.Load<SpriteFont>("TimerFont");
        }

        public void MoveTiles()
        {
            for (int i = 0; i < tiles.Count; i++)
                tiles[i].position.X -= player.velocity.X;
        }
    }
}