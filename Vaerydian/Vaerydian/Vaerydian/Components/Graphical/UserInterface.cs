using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

using Vaerydian.UI;

namespace Vaerydian.Components.Graphical
{
    class UserInterface : IComponent
    {
        private static int u_TypeID;
        private int u_EntityID;

        public UserInterface() { }

        public UserInterface(IUserInterface ui)
        {
            u_UI = ui;
        }

        public int getEntityId()
        {
            return u_EntityID;
        }

        public int getTypeId()
        {
            return u_TypeID;
        }

        public void setEntityId(int entityId)
        {
            u_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            u_TypeID = typeId;
        }

        private IUserInterface u_UI;

        public IUserInterface UI
        {
            get { return u_UI; }
            set { u_UI = value; }
        }

    }
}
