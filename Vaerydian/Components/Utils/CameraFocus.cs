using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

namespace Vaerydian.Components.Utils
{
    public class CameraFocus : IComponent
    {
        private static int c_TypeID;
        private int c_EntityID;

        private int c_FocusRadius;

        public CameraFocus() { }

        public CameraFocus(int focusRadius)
        {
            c_FocusRadius = focusRadius;
        }

        public int getEntityId()
        {
            return c_EntityID;
        }

        public int getTypeId()
        {
            return c_TypeID;
        }

        public int getFocusRadius()
        {
            return c_FocusRadius;
        }

        public void setEntityId(int entityId)
        {
            c_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            c_TypeID = typeId;
        }

        public void setFocusRadius(int focusRadius)
        {
            c_FocusRadius = focusRadius;
        }
    }
}
