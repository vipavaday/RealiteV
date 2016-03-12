using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RealiteV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GremlinsAdventure.Metier
{
    public class Balance:Plateform
    {
        Joint pivot;

        public Balance(World worldFarseer, Vector2 posDepart, Texture2D tex) : base(worldFarseer, posDepart, tex)
        {
            objPhys.IsStatic = false;
            objPhys.BodyType = BodyType.Dynamic;
            objPhys.AngularDamping = 0.8f;
            objPhys.Inertia = 2;
            Body rotor = BodyFactory.CreateCircle(worldFarseer, 0.1f, 3f);
            pivot = JointFactory.CreateRevoluteJoint(worldFarseer,objPhys,rotor,objPhys.Position);
        }
    }
}
