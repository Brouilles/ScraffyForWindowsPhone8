using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using ScraffyForWP.Gameplay.Entity;
using ScraffyForWP.GUI.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScraffyForWP.Gameplay
{
    public enum InGameState { Play, GameOverTransition, GameOver };
    class InGame
    {
        private InGameState m_inGameState;
        private ContentManager m_content;

        //GameOver
        private GameOverMenu m_gameOverMenu = new GameOverMenu();

        //Timer
        private float m_elapsed;
        private int m_timer;

        //Map 
        int m_mapCaseWidth = 256;
        int m_mapCaseHeight = 257;
        private int[,] m_map = { { 0, 0, 0 },
                                 { 0, 0, 0 },
                                 { 0, 0, 0 },
                                 { 0, 0, 0 } };

        //Gameplay
        int m_score = 0;

        //Cats
        private List<EntityCat> m_cat = new List<EntityCat>();

        //Texture
        private Texture2D m_background;
        private Texture2D m_middleBackground;

        //CONSTRUCTOR
        public InGame(ContentManager content, GraphicsDevice graphicsDevice)
        {
            //GameOver
            m_gameOverMenu.initialized(content);

            m_content = content;
            m_inGameState = InGameState.Play;    
        }

        //Load and Unload
        public bool LoadContent()
        {
            m_background = m_content.Load<Texture2D>("InGame/background");
            m_middleBackground = m_content.Load<Texture2D>("InGame/middle");

            m_gameOverMenu.LoadContent();

            return true;
        }

        public bool deallocated()
        {
            return true;
        }

        //Update 
        public void Update(GameTime gameTime, Vector2 screenScale, TouchCollection touchPanelState)
        {
            if (m_inGameState == InGameState.Play)
            {
                //Timer
                m_elapsed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                m_timer -= (int)m_elapsed;

                //Input
                TouchInput(screenScale, touchPanelState);

                //Update Map
                if (m_timer <= -2000)
                {
                    //Verify gameOver or not
                    foreach(int map in m_map)
                    {
                        if (map == 1)
                            m_inGameState = InGameState.GameOverTransition;
                    }

                    if (m_inGameState != InGameState.GameOver && m_inGameState != InGameState.GameOverTransition)
                        m_score++;

                    Array.Clear(m_map, 0, m_map.Length);
                    m_cat.Clear();

                    Random rndNumber = new Random(unchecked((int)DateTime.Now.Ticks));

                    for (int loopX = 0; loopX <= 2; loopX++)
                    {
                        for (int loopY = 0; loopY <= 3; loopY++)
                        {
                            //Changed array
                            if (rndNumber.Next(0, 100) < 50)
                            {
                                m_map[loopY, loopX] = 1;
                            }

                            //Add entity
                            if (m_map[loopY, loopX] == 1 || m_map[loopY, loopX] == 2)
                            {
                                m_cat.Add(new EntityCat());
                                m_cat[m_cat.Count() - 1].initialized(m_content, loopX, loopY, "InGame/Entity/cat", "InGame/Entity/catDead", 0, 238, 229, 246, 229, 246);
                                m_cat[m_cat.Count() - 1].setPos(m_mapCaseWidth * loopX, m_mapCaseHeight * (loopY + 1));
                                m_cat[m_cat.Count() - 1].animState(true);
                            }
                        }
                    }

                    m_timer = 0;
                }

                foreach (EntityCat cat in m_cat)
                {
                    cat.Update(gameTime, touchPanelState, screenScale);
                }
            }
            else if(m_inGameState == InGameState.GameOverTransition)
            {
                m_gameOverMenu.UpdateScore(m_score);
                m_inGameState = InGameState.GameOver;
            }
            else
                m_gameOverMenu.Update(gameTime, screenScale, touchPanelState);
        }

        //Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            if (m_inGameState == InGameState.Play)
            {
                spriteBatch.Draw(m_background, new Vector2(0, 0), Color.White);
                spriteBatch.Draw(m_middleBackground, new Vector2(0, 0), Color.White);

                //Score
                spriteBatch.DrawString(Resource.ImpactBig, "Score: " + m_score, new Vector2(10, 10), Color.DarkGreen);

                foreach (EntityCat cat in m_cat)
                {
                    cat.Draw(spriteBatch);
                }
            }
            else
                m_gameOverMenu.Draw(spriteBatch);
        }

        //Methods
        public void reset()
        {
            m_score = 0;
            Array.Clear(m_map, 0, m_map.Length);
            m_cat.Clear();

            m_inGameState = InGameState.Play;
        }

        public void TouchInput(Vector2 screenScale, TouchCollection touchPanelState)
        {
            foreach (EntityCat cat in m_cat)
            {
                if(cat.TouchInputKill(screenScale, touchPanelState))
                    m_map[cat.getMapPosY(), cat.getMapPosX()] = 2;

                if (cat.getCatState() == CatState.Dead)
                {
                    m_map[cat.getMapPosY(), cat.getMapPosX()] = 0;
                    cat.animState(false);
                }
            }
        }
    }
}
