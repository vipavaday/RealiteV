using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GremlinsAdventure.Menu
{
   public class FadeEffect : ImageEffect
    {

        public float FadeSpeed;
        public bool Increase;

        public FadeEffect()
        {
            FadeSpeed = 1f;
            Increase = false;

        }
        public override void LoadContent(ref Image Image)
        {
            base.LoadContent(ref Image);
        }
        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            if (image.IsActive)
            {
                if (!Increase)
                    image.Alpha -= FadeSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                else
                    image.Alpha += FadeSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (image.Alpha < 0.0f)
                {
                    Increase = true;
                    image.Alpha = 0.0f;
                }
                else if (image.Alpha > 1.0f)
                {
                    Increase = false;
                    image.Alpha = 1.0f;
                }
            }
            else
                image.Alpha = 1.0f;
        }
    }
}
