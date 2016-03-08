using System;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using Leap;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RealiteV;

namespace GremlinsAdventure.LeapMotion
{
    class LeapHandler:Component
    {
        static readonly BodyType typePhys = BodyType.Dynamic;
        static readonly float restitutionPhys = 0.3f;
        static readonly float frictionPhys = 0.05f;

        public bool IsTwoHands { get; set; }

        private Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set
            {
                position.X = value.X;
                position.Y = value.Y;
                objPhys.Position =ConvertUnits.ToSimUnits(position);
            }
        }

        public LeapHandler(ContentManager contentManager,World worldFarseer)
        {
            position = new Vector2(0, 0);
            Texture = contentManager.Load<Texture2D>("crayon");
            objPhys = GetBodyFromTexture(Texture, worldFarseer, Position);
            IsTwoHands = false;
        }

        public override void  DrawAnimation(SpriteBatch spriteBatch)
        {
            if(!IsTwoHands)
                spriteBatch.Draw(Texture,ConvertUnits.ToDisplayUnits(objPhys.Position), null, Color.White, 0, new Vector2(Texture.Width / 2f, Texture.Height / 2f), 1f, SpriteEffects.None, 0f);
        }

        public static Vector2 convertLeapUnits(Vector2 pos)
        {
            float xApp =3000 * pos.X / 400 + 700;
            float yApp = 2500 * pos.Y / 1000 + 2500;

            return new Vector2(xApp, 1800 - pos.Y * 5);
        }

        protected override void SetBodyPhys()
        {
            if (objPhys == null)
                throw new Exception("L'objet physique du leaphandler est null.");
            InitBodyFeatures();
        }

        protected override void InitBodyFeatures()
        {
            objPhys.BodyType = typePhys;
            objPhys.Restitution = restitutionPhys;
            objPhys.Friction = frictionPhys;
            objPhys.Mass = 1000f;
            objPhys.Inertia = 1000f;
            objPhys.CollisionCategories = Category.All;
            objPhys.CollidesWith = Category.All;
        }
    }
}
