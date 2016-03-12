using GremlinsAdventure.Menu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RealiteV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GremlinsAdventure.Metier
{
    public class GamePlayScreen : GameScreen
    {
        Level Level;

        public override void LoadContent()
        {
            base.LoadContent();
            content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");
            XmlManager<Level> levelLoader = new XmlManager<Level>();
            Level = levelLoader.Load("Metier/Level.xml");


            Texture2D tex2 = content.Load<Texture2D>(Level.gremlins2Tex.Path);
            Texture2D tex3 = content.Load<Texture2D>(Level.plateformsTex.Path);
            Texture2D tex1 = content.Load<Texture2D>(Level.gremlins1Tex.Path);

            Level.gremlins1Tex.Texture = tex1;
            Level.gremlins2Tex.Texture = tex2;
            Level.plateformsTex.Texture = tex3;


            Level.GetLevelDefaultConfig(new Vector2(1600, 850),this.content);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            Level.UnloadContent();
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            Level.Update(gameTime);
        }


        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            Level.Draw(spriteBatch);
        }
    }
}
