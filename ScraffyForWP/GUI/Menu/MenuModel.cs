using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScraffyForWP.GUI.Menu
{
    class MenuModel
    {
        protected ContentManager m_contentManager;

        //CONSTRUCTOR
        public MenuModel()
        { }

        public virtual void initialized(ContentManager content)
        {
            m_contentManager = content;
        }

        public virtual void deallocated()
        {
            m_contentManager.Unload();
        }

        //UPDATE
        public virtual void Update(GameTime gameTime, Vector2 m_screenScale, TouchCollection touchPanelState)
        {
            
        }

        //INPUT METHODS
        public virtual void TouchInput()
        { }
    }
}
