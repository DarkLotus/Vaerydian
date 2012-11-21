using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using Glimpse.Controls;

using ECSFramework;

using Vaerydian.Components.Spatials;
using Vaerydian.Components.Graphical;

namespace Vaerydian.UI
{
    class DialogTimer
    {
        private int d_Duration;

        private int d_ElapsedTime;

        private ComponentMapper d_PositionMapper;
        private ComponentMapper d_ViewPortMapper;

        public DialogTimer(int duration, ECSInstance ecsInstance)
        {
            d_Duration = duration;
            d_PositionMapper = new ComponentMapper(new Position(), ecsInstance);
            d_ViewPortMapper = new ComponentMapper(new ViewPort(), ecsInstance);
        }

        /// <summary>
        /// updates the location of the control according to the location of the caller.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="args"></param>
        public void updateHandler(IControl control, InterfaceArgs args)
        {
            d_ElapsedTime += control.ECSInstance.ElapsedTime;

            if (d_ElapsedTime >= d_Duration)
                control.ECSInstance.deleteEntity(control.Owner);

            Position pos = (Position)d_PositionMapper.get(control.Caller);
            ViewPort camera = (ViewPort)d_ViewPortMapper.get(control.ECSInstance.TagManager.getEntityByTag("CAMERA"));

            

            if (pos != null)
            {
                Vector2 pt = pos.Pos - camera.getOrigin();
                control.Bounds = new Rectangle((int)pt.X, (int)pt.Y - control.Bounds.Height - 16, control.Bounds.Width, control.Bounds.Height);
            }
        }

    }
}
