using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WolfSim
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static int SCREENW = 1280, SCREENH = 720;
        public static Random rand = new Random(DateTime.Now.Millisecond);

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private static int screenNum = 0;
        private static Screen[] screenStack = new Screen[32];

        public static void PushScreen(Screen s)
        {
            screenStack[screenNum++] = s;
        }

        public static Screen PopScreen()
        {
            return screenStack[screenNum -= 1];
        }

        public static Screen PeekBack(int ct)
        {
            return screenStack[screenNum - ct];
        }

        public static Screen Peek()
        {
            return screenStack[screenNum - 1];
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            this.graphics.PreferredBackBufferWidth = SCREENW;
            this.graphics.PreferredBackBufferHeight = SCREENH;
            this.IsMouseVisible = true;
            Content.RootDirectory = "Content";
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
            AssMan.LoadStaticAssets(Content);
            PushScreen(new SplashScreen());
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }
            KVMA_Keyboard.Flip();
            KVMA_Mouse.Flip();

            Peek().Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            Peek().Render(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
