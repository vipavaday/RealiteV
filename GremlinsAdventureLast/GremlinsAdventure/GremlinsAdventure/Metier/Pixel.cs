using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GremlinsAdventure.Metier
{
    public class Pixel
    {
        public Texture2D Texture;
        public Vector2 Position;

        public Pixel(Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;
        }

    }
}
