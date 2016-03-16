﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace GremlinsAdventure.Menu
{
    public class ScreenManager : GameScreen
    {
        private static ScreenManager instance;
        [XmlIgnore]
        public Vector2 Dimensions { private set; get; }
        [XmlIgnore]
        public ContentManager Content { private set; get; }
        XmlManager<GameScreen> xmlGameScreenManager;
        [XmlIgnore]
        public GameScreen currentScreen, newScreen;
        [XmlIgnore]
        public GraphicsDevice GraphicDevice;
        [XmlIgnore]
        public SpriteBatch SpriteBatch;

        public Image Image;
        [XmlIgnore]
        public bool IsTransitioning { get; private set; }

        public static ScreenManager Instance
        {
            get
            {
                if (instance == null){

                    XmlManager<ScreenManager> xml = new XmlManager<ScreenManager>();
                    instance = xml.Load("Menu/Load/ScreenManager.xml");
                    //instance = new ScreenManager();
            }
                return instance;
            }
        }


        public void ChangeScreens(string screenName)
        {
            Type ti = Type.GetType("GremlinsAdventure.Menu." + screenName);
            if(ti == null)
             ti = Type.GetType("GremlinsAdventure.Metier." + screenName);
            newScreen = (GameScreen)Activator.CreateInstance(ti);
            Image.IsActive = true;
            Image.FadeEffect.Increase = true;
            Image.Alpha = 0.0f;
            IsTransitioning = true;
        }

        void Transition(GameTime gameTime)
        {
            if (IsTransitioning)
            {
                Image.Update(gameTime);
                if (Image.Alpha == 1.0f)
                {
                    currentScreen.UnloadContent();
                    currentScreen = newScreen;
                    xmlGameScreenManager.Type = currentScreen.Type;
                    if (File.Exists(currentScreen.XmlPath))
                    {
                        //XmlManager<ScreenManager> xml = new XmlManager<ScreenManager>();
                        currentScreen = xmlGameScreenManager.Load(currentScreen.XmlPath);
                    }
                    currentScreen.LoadContent();
                }
                else if (Image.Alpha == 0.0f)
                {
                    Image.IsActive = false;
                    IsTransitioning = false;
                }
            }
        }

        public ScreenManager()
        {
            
            Dimensions = new Vector2(1280, 720);
            currentScreen = new SplashScreen();
            xmlGameScreenManager = new XmlManager<GameScreen>();
            xmlGameScreenManager.Type = currentScreen.Type;
            currentScreen = xmlGameScreenManager.Load("Menu/Load/SplashScreen.xml");

        }

        public void LoadContent(ContentManager Content)
        {
            this.Content = new ContentManager(Content.ServiceProvider, "Content");
            currentScreen.LoadContent();
            Image.LoadContent();
        }
        public void UnloadContent()
        {
            currentScreen.UnloadContent();
            Image.UnloadContent();
        }
        public void Update(GameTime gameTime)
        {
            currentScreen.Update(gameTime);
            Transition(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            currentScreen.Draw(spriteBatch);
            if (IsTransitioning)
                Image.Draw(spriteBatch);

        }

    }
}
