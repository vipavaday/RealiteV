
using FarseerPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RealiteV;
using System.Collections.Generic;
using Leap;
using System;
using GremlinsAdventure.Menu;
using GremlinsAdventure.Metier;

namespace GremlinsGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GremlinsAdventure : Game
    {
        private Controller controller = new Controller();

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public const int WINDOW_WIDTH = 800;
        public const int WINDOW_HEIGHT = 500;
        //World worldFarseer;

        List<Level> listeNiveaux;
        Texture2D mouseTexture;

        //Dictionary<Pixel, Vector2> listPixel = new Dictionary<Pixel, Vector2>();
       
        Vector2 mouseCoordinates;

        public GremlinsAdventure()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferMultiSampling = true;
            
            this.controller = new Controller();
            this.controller.EnableGesture(Gesture.GestureType.TYPECIRCLE);
            ConvertUnits.SetDisplayUnitToSimUnitRatio(64f);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
//            worldFarseer = new World(new Vector2(0f, 9.82f));

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphics.PreferredBackBufferHeight = (int)ScreenManager.Instance.Dimensions.Y;
            graphics.PreferredBackBufferWidth = (int)ScreenManager.Instance.Dimensions.X;

            this.mouseTexture = this.Content.Load<Texture2D>("crayon");
           
           graphics.ApplyChanges();
            // TODO: Add your initialization logic here
         //   listeNiveaux = new List<Level>();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {  
            
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ScreenManager.Instance.GraphicDevice = GraphicsDevice;
            ScreenManager.Instance.SpriteBatch= spriteBatch;
            ScreenManager.Instance.LoadContent(Content);
            
            
/*
            listeNiveaux.Add(new Level(tex1, tex2, tex3));
            
            
            listeNiveaux[0].GetLevelDefaultConfig(worldFarseer, new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT));
            */
          

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            ScreenManager.Instance.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {


            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                if (ScreenManager.Instance.currentScreen.Type == Type.GetType("GremlinsAdventure.Menu.TitleScreen"))
                {
                    this.Exit();
                }else
                {
                    ScreenManager.Instance.ChangeScreens("TitleScreen");
                }
                
            }

            ScreenManager.Instance.Update(gameTime);


            var mouseState = Mouse.GetState();
            this.mouseCoordinates = new Vector2(mouseState.X, mouseState.Y);
          
            // TODO: Add your update logic here
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            
           // listeNiveaux[0].UpdateRotationState();

            //worldFarseer.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);

            base.Update(gameTime);


            
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Turquoise);
            spriteBatch.Begin();
            ScreenManager.Instance.Draw(spriteBatch);
           // this.spriteBatch.Draw(this.mouseTexture, new Rectangle(0, 0, this.mouseTexture.Width, this.mouseTexture.Height), Color.White);

            this.spriteBatch.Draw(this.mouseTexture, this.mouseCoordinates, Color.White);
            // TODO: Add your drawing code here
           

            //listeNiveaux[0].DrawAnimationLevel(spriteBatch);
            GraphicsDevice.Clear(Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
