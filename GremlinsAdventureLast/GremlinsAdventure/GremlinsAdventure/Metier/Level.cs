using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using GremlinsAdventure.LeapMotion;
using GremlinsAdventure.Menu;
using GremlinsAdventure.Metier;
using Leap;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;

namespace RealiteV
{
    public class Level 
    {
        LeapHandler leapHandler;
        static Body boundaries;
        private int nbHands=0;
        public GremlinsAdventure.Menu.Image gremlins1Tex, gremlins2Tex, plateformsTex;

        private bool isWinner = false;


        private GremlinsAdventure.Menu.Image img2;
        private GremlinsAdventure.Menu.Image img;
        SpriteFont font;
        SpriteFont font2;


        [XmlIgnore]
        private Controller controller = new Controller();
        
        public String Nom { get; set; }
        public String Difficulte { get; set; }
        [XmlIgnore]
        public List<Component> Contenu;
        [XmlIgnore]
        public Gremlins grem1, grem2;
        [XmlIgnore]
        World worldFarseer;
        private ContentManager contentManager;
        [XmlIgnore]
        List<List<Pixel>> listoflistPixel = new List<List<Pixel>>();
        [XmlIgnore]
        Texture2D pixel;
        [XmlIgnore]
        Vector2 mouseCoordinates;
        [XmlIgnore]
        ContentManager content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");
        int turn= 0;
        [XmlIgnore]
        Texture2D newTexture;
        [XmlIgnore]
        Body NewobjPhys =null;





        public Level()
        {
            Contenu = new List<Component>();
            worldFarseer = new World(new Vector2(0f, 9.82f));
            //GetLevelDefaultConfig(WorldFarseer,Vector2 windowSize);
            this.pixel = content.Load<Texture2D>("pixel");
        }

        public void SetGremlins(Gremlins grem1,Gremlins grem2)
        {
            this.grem1 = grem1;
            this.grem2 = grem2; 
        }


        public void GetLevelDefaultConfig(Vector2 windowSize, ContentManager content)
        {
            Contenu.RemoveRange(0, Contenu.Count);
            leapHandler = new LeapHandler(content,worldFarseer);
            boundaries = getBounds(windowSize.X, windowSize.Y, worldFarseer);
            Plateform gnd = new Plateform(worldFarseer, new Vector2(405, windowSize.Y - 30), plateformsTex.Texture);
            Water wat1 = new Water(new Vector2(windowSize.X - 125, windowSize.Y - 40), worldFarseer, content);
            Water wat2 = new Water(new Vector2(windowSize.X - 302, windowSize.Y - 40), worldFarseer, content);
            Water wat3 = new Water(new Vector2(windowSize.X - 548, windowSize.Y - 40), worldFarseer, content);
            Water wat4 = new Water(new Vector2(windowSize.X - 795, windowSize.Y - 40), worldFarseer, content);
            Plateform gnd2 = new Balance(worldFarseer, new Vector2(plateformsTex.Texture.Width + 2, windowSize.Y / 2), plateformsTex.Texture);
            this.contentManager = content;
            grem1 = new Gremlins("Guizmo", new Vector2(100, 0), gremlins1Tex.Texture, worldFarseer);
            grem2 = new Gremlins("Daffy", new Vector2(850, 0), gremlins2Tex.Texture, worldFarseer);
            isWinner = false;
            LoadContent();
            Contenu.AddRange(new Component[] { gnd, gnd2, wat1, wat2, wat3, wat4 });
        }

        private void drawAnimationGremlins(SpriteBatch spriteBatch)
        {
            grem1.DrawAnimation(spriteBatch);
            grem2.DrawAnimation(spriteBatch);
        }

        public void DrawAnimationLevel(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Contenu.Count; i++)
            {
                Contenu[i].DrawAnimation(spriteBatch);
            }
            if (grem1 != null && grem2 != null)
                drawAnimationGremlins(spriteBatch);
        }

        public void UpdateRotationState()
        {
            for(int i=0;i<Contenu.Count;i++)
                Contenu[i].SetRotation(Contenu[i].GetRotation());
        }

        public void LoadContent()
        {
            grem1.getBodyPhys().OnCollision += MyOnCollision;

            font = content.Load<SpriteFont>("FatFont");
            img = new GremlinsAdventure.Menu.Image();
            img.Text = "YOU WIN !!!";
            img2 = new GremlinsAdventure.Menu.Image();
            font2 = content.Load<SpriteFont>("NewSpriteFont");
            img2.Text = "Press space \n TO RETRY";


        }

        public bool MyOnCollision(Fixture f1, Fixture f2, Contact contact)
        {

            if (f1.Shape.GetType() == f2.Shape.GetType())
            {
                isWinner = true;
            }

            return true;
        }

        public void UnloadContent()
        {

        }

        public void Update(GameTime gameTime)
        {

            bool isCircle = false, exist = false;
            if (controller.IsConnected)
            {
                Frame instantT = controller.Frame();
                GestureList contentGestures = instantT.Gestures();
                HandList hands = instantT.Hands;
                nbHands = hands.Count;
                if (nbHands == 2)
                    leapHandler.IsTwoHands = true;
                else leapHandler.IsTwoHands = false;
                Finger doigt = hands.Rightmost.Fingers.Frontmost;
               
                    leapHandler.Position = LeapHandler.convertLeapUnits(new Vector2(doigt.StabilizedTipPosition.x, doigt.StabilizedTipPosition.y));
                for (int i = 0; i < contentGestures.Count; i++)
                {
                    if (contentGestures[i].Type == Gesture.GestureType.TYPECIRCLE && hands.Count == 2)
                        isCircle = true;
                }
            }

            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Space) || isCircle)
            {
                worldFarseer.Clear();
                GetLevelDefaultConfig(new Vector2(1600, 850), contentManager);
            }
            UpdateRotationState();

            worldFarseer.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);

            var mouseState = Mouse.GetState();
            this.mouseCoordinates = new Vector2(mouseState.X, mouseState.Y);

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (turn == 0)
                {
                    listoflistPixel.Add(new List<Pixel>());
                    turn = 1;
                }
                if(listoflistPixel.Count > 0){
                if (listoflistPixel[listoflistPixel.Count - 1].Count > 0)
                {
                   
                    if ((mouseCoordinates.X - listoflistPixel[listoflistPixel.Count - 1][listoflistPixel[listoflistPixel.Count - 1].Count -1].Position.X) > 1)
                    {
                        
                        for (int i = (int) listoflistPixel[listoflistPixel.Count - 1][listoflistPixel[listoflistPixel.Count - 1].Count -1].Position.X; i < mouseCoordinates.X; ++i)
                        {
                            exist = false;
                            foreach (Pixel p in listoflistPixel[listoflistPixel.Count - 1])
                            {
                                if (i == p.Position.X && listoflistPixel[listoflistPixel.Count - 1][listoflistPixel[listoflistPixel.Count - 1].Count - 1].Position.Y == p.Position.Y)
                                    exist = true;
                            }
                            if(!exist)
                                listoflistPixel[listoflistPixel.Count - 1].Add(new Pixel(pixel, new Vector2(i,  listoflistPixel[listoflistPixel.Count - 1][listoflistPixel[listoflistPixel.Count - 1].Count -1].Position.Y)));
                        }

                    }
                    else if ((mouseCoordinates.X -  listoflistPixel[listoflistPixel.Count - 1][listoflistPixel[listoflistPixel.Count - 1].Count -1].Position.X) < -1)
                    {
                        
                        for (int i = (int) listoflistPixel[listoflistPixel.Count - 1][listoflistPixel[listoflistPixel.Count - 1].Count -1].Position.X; i > mouseCoordinates.X; --i)
                        {
                            exist = false;
                            foreach (Pixel p in listoflistPixel[listoflistPixel.Count - 1])
                            {
                                if (i == p.Position.X && listoflistPixel[listoflistPixel.Count - 1][listoflistPixel[listoflistPixel.Count - 1].Count - 1].Position.Y == p.Position.Y)
                                    exist = true;
                            }
                            if (!exist)
                                listoflistPixel[listoflistPixel.Count - 1].Add(new Pixel(pixel, new Vector2(i,  listoflistPixel[listoflistPixel.Count - 1][listoflistPixel[listoflistPixel.Count - 1].Count -1].Position.Y)));
                        }
                    }

                    if ((mouseCoordinates.Y -  listoflistPixel[listoflistPixel.Count - 1][listoflistPixel[listoflistPixel.Count - 1].Count -1].Position.Y) > 1)
                    {
                        
                        for (int i = (int) listoflistPixel[listoflistPixel.Count - 1][listoflistPixel[listoflistPixel.Count - 1].Count -1].Position.Y; i < mouseCoordinates.Y; ++i)
                        {
                            exist = false;
                            foreach (Pixel p in listoflistPixel[listoflistPixel.Count - 1])
                            {
                                if (i == p.Position.Y && listoflistPixel[listoflistPixel.Count - 1][listoflistPixel[listoflistPixel.Count - 1].Count - 1].Position.X == p.Position.X)
                                    exist = true;
                            }
                            if (!exist)
                                listoflistPixel[listoflistPixel.Count - 1].Add(new Pixel(pixel, new Vector2( listoflistPixel[listoflistPixel.Count - 1][listoflistPixel[listoflistPixel.Count - 1].Count -1].Position.X, i)));
                        }

                    }
                    else if ((mouseCoordinates.Y -  listoflistPixel[listoflistPixel.Count - 1][listoflistPixel[listoflistPixel.Count - 1].Count -1].Position.Y) < -1)
                    {
                       
                        for (int i = (int) listoflistPixel[listoflistPixel.Count - 1][listoflistPixel[listoflistPixel.Count - 1].Count -1].Position.Y; i > mouseCoordinates.Y; --i)
                        {
                            exist = false;
                            foreach (Pixel p in listoflistPixel[listoflistPixel.Count - 1])
                            {
                                if (i == p.Position.Y && listoflistPixel[listoflistPixel.Count - 1][listoflistPixel[listoflistPixel.Count - 1].Count - 1].Position.X == p.Position.X)
                                    exist = true;
                            }
                            if (!exist)
                                listoflistPixel[listoflistPixel.Count - 1].Add(new Pixel(pixel, new Vector2( listoflistPixel[listoflistPixel.Count - 1][listoflistPixel[listoflistPixel.Count - 1].Count -1].Position.X, i)));
                        }
                    }
                }
                }
                exist = false;
                foreach (Pixel p in listoflistPixel[listoflistPixel.Count - 1])
                {
                    if (mouseCoordinates.Y == p.Position.Y && mouseCoordinates.X == p.Position.X)
                        exist = true;
                }
                if (!exist)
                    listoflistPixel[listoflistPixel.Count - 1].Add(new Pixel(pixel, mouseCoordinates));
            }
            if (mouseState.LeftButton == ButtonState.Released && turn == 1)
            {
                turn = 0;

                CreateTexture2D();

                NewobjPhys = GetBodyFromTexture(newTexture, worldFarseer, new Vector2(200,200));
                NewobjPhys.BodyType = BodyType.Dynamic;
                NewobjPhys.Mass = listoflistPixel[listoflistPixel.Count - 1].Count * 0.5f;
                NewobjPhys.Restitution = 0.2f;
            }

        }

        private void CreateTexture2D()
        {
            var lastListOfPixels = listoflistPixel[listoflistPixel.Count - 1];

            float XMin = lastListOfPixels.Min(p => p.Position.X);
            float XMax = lastListOfPixels.Max(p => p.Position.X);
            float YMin = lastListOfPixels.Min(p => p.Position.Y);
            float YMax = lastListOfPixels.Max(p => p.Position.Y);

            int width = (int)(XMax - XMin + 1);
            int height = (int)(YMax - YMin + 1);
            Color[] data = new Color[(width) * (height)];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = new Color(Color.Transparent,0f);
            }
            Color[] color = new Color[4];

            lastListOfPixels.ForEach(t1x1 =>
            {
                t1x1.Texture.GetData<Color>(color, 0, 1);
                data[(int)(t1x1.Position.Y - YMin) * width + (int)(t1x1.Position.X - XMin)] = color[0];
            });
            newTexture = new Texture2D(ScreenManager.Instance.GraphicDevice, width, height);
            newTexture.SetData(data);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawAnimationLevel(spriteBatch);
            leapHandler.DrawAnimation(spriteBatch);
            if (isWinner)
            {

                spriteBatch.DrawString(font, img.Text, new Vector2((float)(1280 / 2.0) - 300, (float)(720 / 2.0) - 150), Color.Black);

                spriteBatch.DrawString(font2, img2.Text, new Vector2((float)(1280 / 2.0) - 300, (float)(720 / 2.0) - 250), Color.Blue);

            }


            if (listoflistPixel.Count > 0)
            {
                foreach (List<Pixel> l in listoflistPixel)
                    foreach (Pixel p in l)
                    {
                        spriteBatch.Draw(p.Texture, p.Position);
                    }

            }

            if (NewobjPhys != null)
            {
                spriteBatch.Draw(newTexture, ConvertUnits.ToDisplayUnits(NewobjPhys.Position), null, Color.White, 0f, new Vector2(newTexture.Width / 2f, newTexture.Height / 2f), 1f, SpriteEffects.None, 0f);
        
            }
        }


        protected Body GetBodyFromTexture(Texture2D tex, World worldFarseer, Vector2 posDepart)
        {
            uint[] textureData = new uint[tex.Width * tex.Height];
            tex.GetData(textureData);
            Vertices verts = PolygonTools.CreatePolygon(textureData, tex.Width, false);

            Vector2 centroid = -verts.GetCentroid();
            verts.Translate(ref centroid);
            Vector2 _origin = -centroid;
            //verts = SimplifyTools.ReduceByDistance(verts, 4f);

            List<Vertices> contourListe = Triangulate.ConvexPartition(verts, TriangulationAlgorithm.Flipcode);
            Vector2 vertScale = new Vector2(ConvertUnits.ToSimUnits(1));
            foreach (Vertices vertices in contourListe)
            {
                vertices.Scale(ref vertScale);
            }

            return BodyFactory.CreateCompoundPolygon(worldFarseer, contourListe, 1f, ConvertUnits.ToSimUnits(posDepart));
        }


        private Body getBounds(float w, float h, World worldFarseer)
        {
            float width = ConvertUnits.ToSimUnits(w - 4);
            float height = ConvertUnits.ToSimUnits(h -4);

            Vertices bounds = new Vertices(4);
            bounds.Add(new Vector2(0, 0));
            bounds.Add(new Vector2(width, 0));
            bounds.Add(new Vector2(width, height));
            bounds.Add(new Vector2(0, height));

            return BodyFactory.CreateLoopShape(worldFarseer, bounds);
        }

        public Texture2D CreateTexture(int width, int height, Func<int, Color> paint)
        {
            //initialize a texture
            if (height == 0)
            {
                height = 2;
            } if (width == 0)
            {
                width = 2;
            }
            Texture2D texture = new Texture2D(ScreenManager.Instance.GraphicDevice, width, height);

            //the array holds the color for each pixel in the texture
            Color[] data = new Color[(width) * (height)];
            int i = 0;
            
            int difX = (int)listoflistPixel[listoflistPixel.Count - 1][0].Position.X, difY = (int)listoflistPixel[listoflistPixel.Count - 1][0].Position.Y;



            foreach (Pixel p in listoflistPixel[listoflistPixel.Count - 1])
            {

                int ligne = (int)p.Position.Y - difY;
                int colone = (int)p.Position.X - difX;




              //  data[colone + ligne * width] = paint(colone + ligne * width);
                //i++;
            }          

            //set the color
            texture.SetData(data);
            
            return texture;
        }
    }
}
