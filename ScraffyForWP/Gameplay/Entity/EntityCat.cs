using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScraffyForWP.Gameplay.Entity
{
    public enum CatAnim { Frame1, Frame2, Frame3, Frame4, Frame5, Frame6 };
    public enum CatState { Life, DeadAnim, Dead };
    class EntityCat
    {
        //Timer
        private float m_elapsed;
        private int m_timer;

        private int m_mapPosX, m_mapPosY;

        //Texture
        private Texture2D m_catTexture;
        private Texture2D m_catDead;
        private Rectangle m_catRectangle;
        private Rectangle m_catPos;

        private CatState m_catState;

        //Animation
        private CatAnim m_catAnim;
        private bool m_animStart;

        //CONSTRUCTOR
        public EntityCat()
        { }

        public void initialized(ContentManager content, int widthMapPos, int heightMapPos, string texturePath, string texturePathDead, int x, int y, int h, int w, int hCut, int wCut)
        {
            //Set Position
            m_catPos = new Rectangle(x, y, w, h);
            m_catAnim = CatAnim.Frame1;
            m_catState = CatState.Life;
            m_animStart = false;

            m_mapPosX = widthMapPos;
            m_mapPosY = heightMapPos;

            //Set Rectangle
            m_catRectangle = new Rectangle(0, 0, wCut, hCut);

            //Load Texture
            m_catTexture = content.Load<Texture2D>(texturePath);
            m_catDead = content.Load<Texture2D>(texturePathDead);
        }

        //UPDATE
        public void Update(GameTime gameTime, TouchCollection touchPanelState, Vector2 screenScale)
        {   
            //Animation
            if (m_animStart)
            {
                m_elapsed = (float)gameTime.ElapsedGameTime.TotalMilliseconds; //Timer
                m_timer -= (int)m_elapsed;

                if (m_timer <= -70) //Duration
                {
                    if (m_catAnim == CatAnim.Frame1)
                        m_catAnim = CatAnim.Frame2; //Frame 2
                    else if (m_catAnim == CatAnim.Frame2)
                        m_catAnim = CatAnim.Frame3; //Frame 3
                    else if (m_catAnim == CatAnim.Frame3)
                        m_catAnim = CatAnim.Frame4; //Frame 4
                    else if (m_catAnim == CatAnim.Frame4)
                        m_catAnim = CatAnim.Frame5; //Frame 5
                    else if (m_catAnim == CatAnim.Frame5)
                        m_catAnim = CatAnim.Frame6; //Frame 6
                    else if (m_catAnim == CatAnim.Frame6 && m_catState == CatState.DeadAnim)
                        m_catState = CatState.Dead;

                    m_timer = 0; //Reset Timer
                }

            }
        }

        //DRAW
        public void Draw(SpriteBatch spritebatch)
        {
            if (m_animStart)
            {
                if (m_catState == CatState.Life)
                {
                    if (m_catAnim == CatAnim.Frame1)
                        spritebatch.Draw(m_catTexture, m_catPos, m_catRectangle, Color.White);
                    else if (m_catAnim == CatAnim.Frame2)
                        spritebatch.Draw(m_catTexture, m_catPos, new Rectangle(m_catRectangle.Width * 1, m_catRectangle.Y, m_catRectangle.Width, m_catRectangle.Height), Color.White);
                    else if (m_catAnim == CatAnim.Frame3)
                        spritebatch.Draw(m_catTexture, m_catPos, new Rectangle(m_catRectangle.Width * 2, m_catRectangle.Y, m_catRectangle.Width, m_catRectangle.Height), Color.White);
                    else if (m_catAnim == CatAnim.Frame4)
                        spritebatch.Draw(m_catTexture, m_catPos, new Rectangle(m_catRectangle.Width * 3, m_catRectangle.Y, m_catRectangle.Width, m_catRectangle.Height), Color.White);
                    else if (m_catAnim == CatAnim.Frame5)
                        spritebatch.Draw(m_catTexture, m_catPos, new Rectangle(m_catRectangle.Width * 4, m_catRectangle.Y, m_catRectangle.Width, m_catRectangle.Height), Color.White);
                    else if (m_catAnim == CatAnim.Frame6)
                        spritebatch.Draw(m_catTexture, m_catPos, new Rectangle(m_catRectangle.Width * 5, m_catRectangle.Y, m_catRectangle.Width, m_catRectangle.Height), Color.White);
                }
                else
                {
                    if (m_catAnim == CatAnim.Frame1)
                        spritebatch.Draw(m_catDead, m_catPos, m_catRectangle, Color.White);
                    else if (m_catAnim == CatAnim.Frame2)
                        spritebatch.Draw(m_catDead, m_catPos, new Rectangle(m_catRectangle.Width * 1, m_catRectangle.Y, m_catRectangle.Width, m_catRectangle.Height), Color.White);
                    else if (m_catAnim == CatAnim.Frame3)
                        spritebatch.Draw(m_catDead, m_catPos, new Rectangle(m_catRectangle.Width * 2, m_catRectangle.Y, m_catRectangle.Width, m_catRectangle.Height), Color.White);
                    else if (m_catAnim == CatAnim.Frame4)
                        spritebatch.Draw(m_catDead, m_catPos, new Rectangle(m_catRectangle.Width * 3, m_catRectangle.Y, m_catRectangle.Width, m_catRectangle.Height), Color.White);
                    else if (m_catAnim == CatAnim.Frame5)
                        spritebatch.Draw(m_catDead, m_catPos, new Rectangle(m_catRectangle.Width * 4, m_catRectangle.Y, m_catRectangle.Width, m_catRectangle.Height), Color.White);
                    else if (m_catAnim == CatAnim.Frame6)
                        spritebatch.Draw(m_catDead, m_catPos, new Rectangle(m_catRectangle.Width * 5, m_catRectangle.Y, m_catRectangle.Width, m_catRectangle.Height), Color.White);
                }
            }
        }

        //Methods
        public bool TouchInputKill(Vector2 screenScale, TouchCollection touchPanelState)
        {
            if (m_animStart)
            {
                //Collision
                foreach (TouchLocation tl in touchPanelState)
                {
                    if (tl.State == TouchLocationState.Pressed)
                    {
                        if (tl.Position.X > m_catPos.X / screenScale.X && tl.Position.X < m_catPos.X / screenScale.X + m_catRectangle.Width / screenScale.X /* X */ 
                            && tl.Position.Y > m_catPos.Y / screenScale.Y && tl.Position.Y < m_catPos.Y / screenScale.Y + m_catRectangle.Height / screenScale.Y /* Y */)
                        {
                            if (m_catState == CatState.Life)
                            {
                                m_catState = CatState.DeadAnim;
                                m_catAnim = CatAnim.Frame1;

                                return true;
                            }
                            else 
                                return false;
                        }
                        else
                            return false;
                    }
                    else
                        return false;
                }
            }
            else
                return false;

            return false;
        }

        public void setPos(int x, int y)
        {
            m_catPos.X = x;
            m_catPos.Y = y;
        }

        public void animState(bool anim)
        {
            m_animStart = anim;
        }

        public CatState getCatState()
        {
            return m_catState;
        }
        public int getMapPosX()
        {
            return m_mapPosX;
        }

        public int getMapPosY()
        {
            return m_mapPosY;
        }
    }
}
