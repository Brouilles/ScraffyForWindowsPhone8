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
    class GameOverMenu : MenuModel
    {
        //Texture
        private Texture2D m_background;
        private Texture2D m_logo;

        //Button
        private ButtonTexture m_restartButton = new ButtonTexture();
        private ButtonTexture m_exitButton = new ButtonTexture();

        //Gameplay
        private string m_bestScoreKills;

        //CONSTRUCTOR
        public GameOverMenu()
        { }

        public override void initialized(ContentManager content)
        {
            m_contentManager = content;
        }

        //Load and Unload
        public bool LoadContent()
        {
            //Texture
            m_background = m_contentManager.Load<Texture2D>("GUI/GameOver/background");
            m_logo = m_contentManager.Load<Texture2D>("GUI/GameOver/gameover");

            //Button
            m_restartButton.initialized(m_contentManager, "GUI/GameOver/Button/restart", 139, 517, 72, 440, 74, 440, 440, 0);
            m_exitButton.initialized(m_contentManager, "GUI/GameOver/Button/exit", 139, 807, 72, 440, 74, 440, 440, 0);

            ReadFileContentsAsync("bestScore.txt");

            return true;
        }

        //UPDATE
        public void Update(GameTime gameTime, Vector2 screenScale, TouchCollection touchPanelState)
        {
            foreach (TouchLocation tl in touchPanelState)  
            {
                //Restart
                if (m_restartButton.Collision((int)(tl.Position.X), (int)(tl.Position.Y), screenScale))
                {
                    if (tl.State == TouchLocationState.Pressed)
                        m_restartButton.Update(true);
                    else if (tl.State == TouchLocationState.Released)
                    {
                        Game1.MenuChanged = true;
                        Game1.MenuList = menuList.InGameReset;
                        m_restartButton.Update(false);
                    }
                }

                //Exit
                if (m_exitButton.Collision((int)(tl.Position.X), (int)(tl.Position.Y), screenScale))
                {
                    if (tl.State == TouchLocationState.Pressed)
                        m_exitButton.Update(true);
                    else if (tl.State == TouchLocationState.Released)
                    {
                        Game1.MenuChanged = true;
                        Game1.MenuList = menuList.MainMenu;
                        m_exitButton.Update(false);
                    }
                }
            }
        }

        //DRAW
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(m_background, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(m_logo, new Vector2(16, 127), Color.White);

            m_restartButton.Draw(spriteBatch);
            m_exitButton.Draw(spriteBatch);
        }

        //Methods
        public void UpdateScore(int newScore)
        {
            if (Convert.ToInt32(m_bestScoreKills) < newScore)
            {
                string scoreSave = Convert.ToString(newScore);
                WriteDataToFileAsync("bestScore.txt", scoreSave);
            }
        }

        public async Task WriteDataToFileAsync(string fileName, string content)
        {
            byte[] data = Encoding.UTF8.GetBytes(content);

            var folder = ApplicationData.Current.LocalFolder;
            var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

            using (var s = await file.OpenStreamForWriteAsync())
            {
                await s.WriteAsync(data, 0, data.Length);
            }
        }

        public async void ReadFileContentsAsync(string fileName)
        {
            var folder = ApplicationData.Current.LocalFolder;

            try
            {
                var file = await folder.OpenStreamForReadAsync(fileName);

                using (var streamReader = new StreamReader(file))
                {
                    string bestScoreString = streamReader.ReadLine();
                    m_bestScoreKills = bestScoreString;
                }
            }
            catch (Exception)
            {
                m_bestScoreKills = "0";
            }
        }
    }
}
