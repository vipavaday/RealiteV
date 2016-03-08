using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RealiteV
{
   public class Plateform:Component
    {
        static readonly bool isStaticPhys = true;
        static readonly float restitutionPhys = 0.3f;
        static readonly float frictionPhys = 0.5f;

        public Plateform(World worldFarseer, Vector2 posDepart,Texture2D tex)
        {
            Texture = tex;
            objPhys = BodyFactory.CreateRectangle(worldFarseer, ConvertUnits.ToSimUnits(Texture.Width), ConvertUnits.ToSimUnits(Texture.Height), 1f, ConvertUnits.ToSimUnits(posDepart));
            SetBodyPhys();
        }

        protected override void SetBodyPhys()
        {  
            if (objPhys == null)
                throw new Exception("L'objet physique de type Plateforme est null.");
            InitBodyFeatures();
        }

        protected override void InitBodyFeatures()
        {
            objPhys.IsStatic = isStaticPhys;
            objPhys.AngularDamping = 0.1f;
            objPhys.Restitution = restitutionPhys;
            objPhys.Friction = frictionPhys;
            objPhys.LinearDamping = 0f;
        }
    }
}
