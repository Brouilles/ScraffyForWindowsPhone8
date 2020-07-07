//MICROSOFT
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
//SYSTEM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScraffyForWP.GUI.Widget
{
    public enum ButtonState { attend, pressed };
    class ButtonTexture
    {
        //Texture
        private Texture2D m_buttonTexture;
        private Rectangle m_buttonRectangle;
        private Rectangle m_buttonPos;

        private Rectangle m_cutRectangle;
        private ButtonState m_buttonState;

        //CONSTRUCTOR
        public ButtonTexture()
        { }

        public void initialized(ContentManager content, string texturePath, int x, int y, int h, int w, int hCut, int wCut, int xCut, int yCut)
        {
            m_buttonState = ButtonState.attend;

            //Set Position
            m_buttonPos = new Rectangle(x, y, w, h);

            //Set Rectangle
            m_buttonRectangle = new Rectangle(0, 0, wCut, hCut);
            m_cutRectangle = new Rectangle(xCut, yCut, w, h);

            //Load Texture
            m_buttonTexture = content.Load<Texture2D>(texturePath);
        }

        //UPDATE
        public void Update(bool choose) //For gamepad
        {
            if (choose)
                m_buttonState = ButtonState.pressed;
            else
                m_buttonState = ButtonState.attend;
        }

        //DRAW
        public void Draw(SpriteBatch spritebatch)
        {
            if (m_buttonState == ButtonState.attend)
                spritebatch.Draw(m_buttonTexture, m_buttonPos, m_buttonRectangle, Color.White);
            else if (m_buttonState == ButtonState.pressed)
                spritebatch.Draw(m_buttonTexture, m_buttonPos, m_cutRectangle, Color.White);
        }

        //Methods
        public bool Collision(int mouseX, int mouseY, Vector2 screenScale)
        {
            if (mouseX > m_buttonPos.X / screenScale.X && mouseX < m_buttonPos.X / screenScale.X + m_buttonRectangle.Width / screenScale.X /* X */ && mouseY > m_buttonPos.Y / screenScale.Y && mouseY < m_buttonPos.Y / screenScale.Y + m_buttonRectangle.Height / screenScale.Y /* Y */)
                return true;
            else
                return false;
        }
    }
}
