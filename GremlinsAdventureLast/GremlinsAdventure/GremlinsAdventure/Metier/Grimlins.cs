using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealiteV
{
    public class Gremlins : Component
    {
        static readonly BodyType typePhys = BodyType.Dynamic;
        static readonly float restitutionPhys = 0.1f;
        static readonly float frictionPhys = 0.5f;

        private Vector2 posDepart;
        public Vector2 PosDepart { get; set; }

        public String Nom { get; set; }

        public Gremlins(String nom,Vector2 posDepart,Texture2D tex ,World worldFarseer)
        {
            Nom = nom;
            Texture = tex;
            objPhys = BodyFactory.CreateCircle(worldFarseer, ConvertUnits.ToSimUnits(Texture.Width/ 2f), 1f, ConvertUnits.ToSimUnits(posDepart));
            SetBodyPhys();
            this.posDepart = posDepart;
        }

        protected override void SetBodyPhys()
        {
            if (objPhys == null)
                throw new Exception("L'objet physique de " + Nom + " est null.");
            InitBodyFeatures();
        }
        public Body getBodyPhys()
        {
            return objPhys;
        }

        protected override void InitBodyFeatures()
        {
            objPhys.BodyType = typePhys;
            objPhys.Restitution = restitutionPhys;
            objPhys.Friction = frictionPhys;
        }
    }
}
