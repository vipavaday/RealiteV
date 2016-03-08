using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Common.PolygonManipulation;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace RealiteV
{
    public abstract class Component
    {
        protected Body objPhys;
        protected float rotation = 0f;
        protected Vector2 _origin, vertScale;

        protected List<Texture2D> textures;
        public List<Texture2D> Textures { get; set; }

        private Texture2D texture;

        public Texture2D Texture {
            get { return texture; }
            set
            {
                if (value == null)
                    throw new Exception("Texture non chargée.");
                this.texture = value;
            }
        }

        protected abstract void SetBodyPhys();

        public virtual void DrawAnimation(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, ConvertUnits.ToDisplayUnits(objPhys.Position), null, Color.White, rotation, new Vector2(Texture.Width/2f,Texture.Height/2f), 1f, SpriteEffects.None, 0f);
        }

        protected abstract void InitBodyFeatures();

        public void SetRotation(float rotation)
        {
            this.rotation = rotation;
        }

        public float GetRotation()
        {
            return objPhys.Rotation;
        }

        protected Body GetBodyFromTexture(Texture2D tex, World worldFarseer, Vector2 posDepart)
        {
            uint[] textureData = new uint[tex.Width * tex.Height];
            tex.GetData(textureData);
            Vertices verts = PolygonTools.CreatePolygon(textureData, tex.Width, false);

            Vector2 centroid = -verts.GetCentroid();
            verts.Translate(ref centroid);
            _origin = -centroid;
            //verts = SimplifyTools.ReduceByDistance(verts, 4f);
            if (verts.CheckPolygon() != PolygonError.NoError)
            {
                Console.WriteLine("Pb avec les vertices pour les collisions.");
            }
            List<Vertices> contourListe = Triangulate.ConvexPartition(verts, TriangulationAlgorithm.Flipcode);
            vertScale = new Vector2(ConvertUnits.ToSimUnits(1));
            foreach (Vertices vertices in contourListe)
            {
                vertices.Scale(ref vertScale);
            }

            return BodyFactory.CreateCompoundPolygon(worldFarseer, contourListe, 1f, ConvertUnits.ToSimUnits(posDepart));
        }

        protected Body GetNonConvexBodyFromTexture(Texture2D tex, World worldFarseer, Vector2 posDepart)
        {
            uint[] textureData = new uint[tex.Width * tex.Height];
            tex.GetData(textureData);
            List <Vertices> verts = PolygonTools.CreatePolygon(textureData, tex.Width, 0f,(byte)5,true,true);

            return BodyFactory.CreateCompoundPolygon(worldFarseer, verts, 1f, ConvertUnits.ToSimUnits(posDepart));
        }
    }
}
