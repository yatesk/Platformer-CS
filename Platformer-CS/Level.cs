using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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


        public Player player = new Player(500, 500);

        public int screen_width = 1200;
        public int screen_height = 800;

        public List<Tile> tiles = new List<Tile>();

        ContentManager content;


        public Level(ContentManager Content)
        {
            background_xy1 = new Vector2(0, 0);
            background_xy2 = new Vector2(1200, 0);

            content = Content;
     
            LoadContent();
            LoadLevel();
        }

        public void Update()
        {
            // only move camera if play is in middle of screen and going right
            if (player.velocity.X > 0 && player.position.X >= screen_width / 2)
            {
                background_xy1.X -= player.velocity.X;
                background_xy2.X -= player.velocity.X;

                if (background_xy1.X <= -screen_width)
                    background_xy1.X = screen_width;

                if (background_xy2.X <= -screen_width)
                    background_xy2.X = screen_width;
            }
            else
                player.position.X += player.velocity.X;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background_image, background_xy1);
            spriteBatch.Draw(background_image, background_xy2);

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
                {
                    level.Add(line.ToCharArray());
                }
            }

            for (int i = 0; i < level.Count; i++)
                for (int j = 0; j < level[i].Length; j++)
                {
                    if (level[i][j] == 'B')
                        tiles.Add(new Tile(64 * j, 64 * i, 64, 64, "brick.png"));
                }
        }

        public void LoadContent()
        {
            //background_image = new this.content.Load<Texture2D>("level2");
            //player.image = content.Load<Texture2D>("player2");
        }
    }
}