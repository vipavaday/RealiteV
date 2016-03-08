using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace GremlinsAdventure.Menu
{
    

    public class SplashScreen : GameScreen
    {
        public Image Image;
        public Image Image2;

        public override void LoadContent()
        {
            base.LoadContent();
            Image.LoadContent();
            Image2.LoadContent();
        }
        public override void UnloadContent()
        {
            base.UnloadContent();
            Image.UnloadContent();
            Image2.UnloadContent();
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            Image.Update(gameTime);
            Image2.Update(gameTime);

            if (InputManager.Instance.KeyPressed(Keys.Enter,Keys.Z))
                ScreenManager.Instance.ChangeScreens("TitleScreen");
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch);
            Image2.Draw(spriteBatch);

        }

    }
}
