using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Common.PolygonManipulation;
using FarseerPhysics.DebugView;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RealiteV;
using System;
using System.Collections.Generic;

namespace GremlinsAdventure.Metier
{
    class Water : Component
    {
        static readonly BodyType typePhys = BodyType.Static;
        static readonly float restitutionPhys = -20.00f;
        static readonly float frictionPhys = 2f;
        private int currentTexture = 0, cursor = 0;

        public Water(Vector2 posDepart, World worldFarseer, ContentManager contentManager)
        {

            textures = new List<Texture2D>(new List<Texture2D>{ contentManager.Load<Texture2D>("vague"),
                                                              contentManager.Load<Texture2D>("vague2"),
                                                              contentManager.Load<Texture2D>("vague3"),
                                                              contentManager.Load<Texture2D>("vague4")});
            objPhys = GetBodyFromTexture(textures[currentTexture], worldFarseer, posDepart);
            SetBodyPhys();
        }

        protected override void SetBodyPhys()
        {
            if (objPhys == null)
                throw new Exception("L'objet physique de type eau est null.");
            InitBodyFeatures();
        }

        protected override void InitBodyFeatures()
        {
            objPhys.BodyType = typePhys;
            objPhys.Restitution = restitutionPhys;
            objPhys.Friction = frictionPhys;
        }

        public override void DrawAnimation(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textures[currentTexture], ConvertUnits.ToDisplayUnits(objPhys.Position), null, Color.White, rotation, _origin, 1.009f, SpriteEffects.None, 0f);
            if (cursor == 25)
            {
                currentTexture = (currentTexture + 1) % textures.Count;
                cursor = -1;
            }
            cursor++;
        }
    }
}
