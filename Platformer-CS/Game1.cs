﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Platformer_CS
{
    public class Game1 : Game
    {
        private int screenWidth = 1200;
        private int screenHeight = 800;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Level level;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            Content.RootDirectory = "Content";
            this.Window.Title = "Platformer CS"; 
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();   
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            level = new Level(Content);

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Escape))
                Exit();

            if (state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.A))
                level.player.Left();
            if (state.IsKeyDown(Keys.Right) || state.IsKeyDown(Keys.D))
                level.player.Right();
            if (state.IsKeyDown(Keys.Down) || state.IsKeyDown(Keys.Up) || state.IsKeyDown(Keys.W) || state.IsKeyDown(Keys.S) || state.IsKeyDown(Keys.Space))
                level.player.Jump();


            if (state.IsKeyUp(Keys.Left) && state.IsKeyUp(Keys.A) && level.player.velocity.X < 0)
                level.player.Stop();
            if (state.IsKeyUp(Keys.Right) && state.IsKeyUp(Keys.D) && level.player.velocity.X > 0)
                level.player.Stop();

            level.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();

            level.Draw(spriteBatch);
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
