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
        public Texture2D background_image;

        public Vector2 background_xy1;
        public Vector2 background_xy2;  // max screen width

        public Player player = new Player(400, 400, 32, 64);

        public int screen_width = 1200;
        public int screen_height = 800;

        public List<Tile> tiles = new List<Tile>();

        public Dictionary<char, Texture2D> tileTextures = new Dictionary<char, Texture2D>();

        ContentManager content;


        public Level(ContentManager Content)
        {
            background_xy1 = new Vector2(0, 0);
            background_xy2 = new Vector2(screen_width, 0);

            content = Content;

            LoadContent();
            LoadLevel();
        }

        public void Update()
        {
            player.onPlatform = player.isOnPlatform(this);

            player.Gravity();

            if (player.position.X < 0)
                player.position.X = 0;

            Rectangle playerBoundingBox = new Rectangle((int)(player.position.X + player.velocity.X), (int)(player.position.Y + player.velocity.Y), player.width, player.height);

            for (int i = 0; i < tiles.Count; i++)
            {
                Rectangle tileBoundingBox = new Rectangle((int)tiles[i].position.X, (int)tiles[i].position.Y, tiles[i].width, tiles[i].height);

                Rectangle intersectArea = Rectangle.Intersect(playerBoundingBox, tileBoundingBox);

                //System.Diagnostics.Debug.WriteLine(intersectArea);

                if (intersectArea.X > 0 || intersectArea.Y > 0)
                {
                    if (player.velocity.X > 0)
                    {
                        player.position.X -= intersectArea.Width;
                    }
                    else
                    {
                        player.position.X += intersectArea.Width;
                    }

                    player.velocity.X = 0;


                    if (player.velocity.Y > 0)
                    {
                        player.position.X -= intersectArea.Height;
                    }
                    else
                    {
                        player.position.X += intersectArea.Height;
                    }

                    player.velocity.Y = 0;
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
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background_image, background_xy1);
            spriteBatch.Draw(background_image, background_xy2);

            for (int i = 0; i < tiles.Count; i++)
                spriteBatch.Draw(tileTextures[tiles[i].name], tiles[i].position);

            player.Draw(spriteBatch);
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
                        tiles.Add(new Tile(new Vector2(64 * j, 64 * i), 64, 64, 'B'));

                    if (level[i][j] == 'F')
                        tiles.Add(new Tile(new Vector2(64 * j, 64 * i), 64, 64, 'F'));

                    if (level[i][j] == 'Q')
                        tiles.Add(new Tile(new Vector2(64 * j, 64 * i), 64, 64, 'Q'));

                    if (level[i][j] == 'P')
                        tiles.Add(new Tile(new Vector2(64 * j, 64 * i), 128, 64, 'P'));

                    if (level[i][j] == 'S')
                        tiles.Add(new Tile(new Vector2(64 * j, 64 * i), 64, 64, 'S'));

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
            player.image = content.Load<Texture2D>("player2");

            tileTextures.Add('B', content.Load<Texture2D>("brick"));
            tileTextures.Add('F', content.Load<Texture2D>("floor1"));
            tileTextures.Add('Q', content.Load<Texture2D>("question"));
            tileTextures.Add('P', content.Load<Texture2D>("pipe"));
            tileTextures.Add('S', content.Load<Texture2D>("stair"));
        }

        public void MoveTiles()
        {
            for (int i = 0; i < tiles.Count; i++)
                tiles[i].position.X -= player.velocity.X;
        }
    }
}