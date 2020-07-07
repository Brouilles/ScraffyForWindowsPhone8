using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScraffyForWP.GUI.Menu
{
    class Credit : MenuModel
    {
        //Texture
        private Texture2D m_background;
        private Texture2D m_aubegaLogo;

        //CONSTRUCTOR
        public Credit()
        { }

        //Load and Unload
        public bool LoadContent()
        {
            //Texture
            m_background = m_contentManager.Load<Texture2D>("GUI/Credit/background");
            m_aubegaLogo = m_contentManager.Load<Texture2D>("GUI/Credit/aubegaLogo");

            return true;
        }

        //UPDATE
        public void Update(TouchCollection touchPanelState)
        {
            foreach (TouchLocation tl in touchPanelState)
            {
                if (tl.State == TouchLocationState.Released)
                {
                    Game1.MenuChanged = true;
                    Game1.MenuList = menuList.MainMenu;
                }
            }
        }

        //DRAW
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(m_background, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(m_aubegaLogo, new Vector2(6, 1178), Color.White);

            //DEZEIRAUD Gaëtan
            spriteBatch.DrawString(Resource.ImpactBig, "DEZEIRAUD Gaëtan", new Vector2(196, 556+10), Color.White);
            spriteBatch.DrawString(Resource.ImpactLittle, "Engine programmer && Gameplay \nprogrammer && Gameplay designer", new Vector2(200, 614+10), Color.White);

            //LynniHappy
            spriteBatch.DrawString(Resource.ImpactBig, "LynniHappy", new Vector2(196, 664+10), Color.White);
            spriteBatch.DrawString(Resource.ImpactLittle, "Graphic artist && Gameplay designer", new Vector2(200, 722+10), Color.White);

            //REDON Emile
            spriteBatch.DrawString(Resource.ImpactBig, "REDON Emile", new Vector2(196, 742+10), Color.White);
            spriteBatch.DrawString(Resource.ImpactLittle, "Sound designer && Music Composer", new Vector2(200, 800+10), Color.White);

            //RETAUD Rémi
            spriteBatch.DrawString(Resource.ImpactBig, "RETAUD Rémi", new Vector2(196, 820+10), Color.White);
            spriteBatch.DrawString(Resource.ImpactLittle, "Co-creator and consultant \non Scraffy (history && music)", new Vector2(200, 878+10), Color.White);
        }
    }
}
