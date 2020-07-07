using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using ScraffyForWP.GUI.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ScraffyForWP.GUI.Menu
{
    public enum ScraffyAnim { Frame1, Frame2, Frame3, Frame4 };
    class MainMenu : MenuModel
    {
        //Timer
        private float m_elapsed, m_scraffyElapsed;
        private int m_timer, m_scraffyTimer;

        //Score
        private string m_bestScore;

        //Texture
        private Texture2D m_background;
        private Texture2D m_logo;
        private Texture2D m_foreground;

        private Texture2D m_cloud1, m_cloud2;
        private Vector2 m_cloud1Pos, m_cloud2Pos;

        private Texture2D m_scraffyFrame1, m_scraffyFrame2, m_scraffyFrame3;
        private ScraffyAnim m_scraffyAnim;

        //Button
        private ButtonTexture m_playButton = new ButtonTexture();
        private ButtonTexture m_scoresButton = new ButtonTexture();
        private ButtonTexture m_optionButton = new ButtonTexture();
        private ButtonTexture m_creditButton = new ButtonTexture();

        //CONSTRUCTOR
        public MainMenu()
        { }

        //METHODS
        public override void initialized(ContentManager content)
        {
            base.initialized(content);
        }

        //Load and Unload
        public bool LoadContent()
        {
            //Texture
            m_background = m_contentManager.Load<Texture2D>("GUI/MainMenu/background");
            m_logo = m_contentManager.Load<Texture2D>("GUI/MainMenu/ScraffyLogo");
            m_foreground = m_contentManager.Load<Texture2D>("GUI/MainMenu/foreground-light");

            m_cloud1 = m_contentManager.Load<Texture2D>("GUI/MainMenu/cloud1");
            m_cloud1Pos = new Vector2(0, 312);

            m_cloud2 = m_contentManager.Load<Texture2D>("GUI/MainMenu/cloud2");
            m_cloud2Pos = new Vector2(774, 359);

            m_scraffyFrame1 = m_contentManager.Load<Texture2D>("GUI/MainMenu/Scraffy/frame1");
            m_scraffyFrame2 = m_contentManager.Load<Texture2D>("GUI/MainMenu/Scraffy/frame2");
            m_scraffyFrame3 = m_contentManager.Load<Texture2D>("GUI/MainMenu/Scraffy/frame3");

            m_scraffyAnim = ScraffyAnim.Frame1;

            //Ini Best Score
            ReadFileContentsAsync("bestScore.txt");

            //Ini Button
            m_playButton.initialized(m_contentManager, "GUI/MainMenu/Button/playButtonTexture", 320, 520, 72, 440, 74, 440, 440, 0);
            m_scoresButton.initialized(m_contentManager, "GUI/MainMenu/Button/scoresButtonTexture", 320, 520 + 150, 72, 440, 74, 440, 440, 0);
            m_optionButton.initialized(m_contentManager, "GUI/MainMenu/Button/optionButtonTexture", 320, 520 + 150 * 2, 72, 440, 74, 440, 440, 0);
            m_creditButton.initialized(m_contentManager, "GUI/MainMenu/Button/creditButtonTexture", 320, 520 + 150 * 3, 72, 440, 74, 440, 440, 0);

            return true;
        }

        //UPDATE
        public override void Update(GameTime gameTime, Vector2 screenScale, TouchCollection touchPanelState)
        {
            foreach (TouchLocation tl in touchPanelState)  
            {
                //Play
                if (m_playButton.Collision((int)(tl.Position.X), (int)(tl.Position.Y), screenScale))
                {
                    if (tl.State == TouchLocationState.Pressed)
                        m_playButton.Update(true);
                    else if (tl.State == TouchLocationState.Released)
                    {
                        Game1.MenuChanged = true;
                        Game1.MenuList = menuList.InGameReset;
                        m_playButton.Update(false);
                    }
                }

                //Credit
                if (m_creditButton.Collision((int)(tl.Position.X), (int)(tl.Position.Y), screenScale))
                {
                    if (tl.State == TouchLocationState.Pressed)
                        m_creditButton.Update(true);
                    else if (tl.State == TouchLocationState.Released)
                    {
                        Game1.MenuChanged = true;
                        Game1.MenuList = menuList.CreditMenu;
                        m_creditButton.Update(false);
                    }
                }
            }   
        }

        //DRAW
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Cloud animation
            m_elapsed = (float)gameTime.ElapsedGameTime.TotalMilliseconds; //Timer
            m_timer -= (int)m_elapsed;

            if (m_cloud2Pos.X < -464)
            {
                m_cloud1Pos.X = 768;
                m_cloud2Pos.X = 768 + 774;
            }

            if (m_timer <= -30) //Duration
            {
                if (m_cloud1Pos.X > -464)
                    m_cloud1Pos.X -= 1;

                m_cloud2Pos.X -= 1;
                m_timer = 0; //Reset Timer
            }

            //Scraffy animation
            m_scraffyElapsed = (float)gameTime.ElapsedGameTime.TotalMilliseconds; //Timer
            m_scraffyTimer -= (int)m_scraffyElapsed;

            if (m_scraffyTimer <= -370) //Duration
            {
                if (m_scraffyAnim == ScraffyAnim.Frame1)
                    m_scraffyAnim = ScraffyAnim.Frame2; //Frame 2
                else if (m_scraffyAnim == ScraffyAnim.Frame2)
                    m_scraffyAnim = ScraffyAnim.Frame3; //Frame 3
                else if (m_scraffyAnim == ScraffyAnim.Frame3)
                    m_scraffyAnim = ScraffyAnim.Frame4; //Frame 2
                else if (m_scraffyAnim == ScraffyAnim.Frame4)
                    m_scraffyAnim = ScraffyAnim.Frame1; //Frame 1

                m_scraffyTimer = 0; //Reset Timer
            }

            spriteBatch.Draw(m_background, new Vector2(0, 0), Color.White);

            spriteBatch.Draw(m_cloud1, m_cloud1Pos, Color.White);
            spriteBatch.Draw(m_cloud2, m_cloud2Pos, Color.White);

            //Scraffy
            if (m_scraffyAnim == ScraffyAnim.Frame1)
                spriteBatch.Draw(m_scraffyFrame1, new Vector2(0, 584), Color.White);
            else if(m_scraffyAnim == ScraffyAnim.Frame2 || m_scraffyAnim == ScraffyAnim.Frame4)
                spriteBatch.Draw(m_scraffyFrame2, new Vector2(0, 584), Color.White);
            else if (m_scraffyAnim == ScraffyAnim.Frame3)
                spriteBatch.Draw(m_scraffyFrame3, new Vector2(0, 584), Color.White);

            spriteBatch.Draw(m_foreground, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(m_logo, new Vector2(16, 127), Color.White);

            //Button
            m_playButton.Draw(spriteBatch);
            m_scoresButton.Draw(spriteBatch);
            m_optionButton.Draw(spriteBatch);
            m_creditButton.Draw(spriteBatch);

            //Score
            spriteBatch.DrawString(Resource.ImpactBig, "Best Score: " + m_bestScore, new Vector2(50, 310), Color.DarkGreen);
        }

        //Methods
        public async void ReadFileContentsAsync(string fileName)
        {
            var folder = ApplicationData.Current.LocalFolder;

            try
            {
                var file = await folder.OpenStreamForReadAsync(fileName);

                using (var streamReader = new StreamReader(file))
                {
                    string bestScoreString = streamReader.ReadLine();
                    m_bestScore = bestScoreString;
                }
            }
            catch (Exception)
            {
                m_bestScore = "0";
            }
        }
    }
}
