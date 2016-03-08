using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GremlinsAdventure.Menu
{
    public class MenuManager
    {

        Menu menu;
        bool isTransitionning;

        void Transition(GameTime gameTime)
        {
            for (int i = 0; i < menu.Items.Count; i++)
            {
                menu.Items[i].Image.Update(gameTime);
                float first = menu.Items[0].Image.Alpha;
                float last = menu.Items[menu.Items.Count - 1].Image.Alpha;

                if (first == 0.0f && last == 0.0f)
                    menu.ID = menu.Items[menu.ItemNumber].LinkID;
                else if (first == 1.0f && last == 1.0f)
                {
                    isTransitionning = false;
                    foreach (MenuItem item in menu.Items)
                        item.Image.RestoreEffects();
                }
            }
        }

        public MenuManager()
        {
            menu = new Menu();
            menu.OnMenuChanged += menu_OnMenuChanged;
        }

        void menu_OnMenuChanged(object sender, EventArgs e)
        {
            XmlManager<Menu> xmlMenuManager = new XmlManager<Menu>();
            menu.UnloadContent();

            menu = xmlMenuManager.Load(menu.ID);
            menu.LoadContent();
            menu.OnMenuChanged += menu_OnMenuChanged;
            menu.Transition(0.0f);

            foreach (MenuItem item in menu.Items)
            {

                item.Image.StoreEffects();
                item.Image.ActivateEffect("FadeEffect");
            }
        }

        public void LoadContent(string menuPath)
        {
            if (menuPath != String.Empty)
                menu.ID = menuPath;
        }

        public void UnloadContent()
        {
            menu.UnloadContent();
        }
        public void Update(GameTime gameTime)
        {
            if(!isTransitionning)
               menu.Update(gameTime);
            if (InputManager.Instance.KeyPressed(Keys.Enter) && !isTransitionning) { 
                isTransitionning =true;
                if (menu.Items[menu.ItemNumber].LinkType == "Screen")
                {
                    ScreenManager.Instance.ChangeScreens(menu.Items[menu.ItemNumber].LinkID);
                }
                else
                {
                    isTransitionning = true;
                    menu.Transition(1.0f);
                    foreach (MenuItem item in menu.Items)
                    {
                        item.Image.StoreEffects();
                        item.Image.ActivateEffect("FadeEffect");
                    }
                }
            }
            Transition(gameTime);

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            menu.Draw(spriteBatch);
        }
    }
}
