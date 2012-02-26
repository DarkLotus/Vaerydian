using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using ECSFramework;
using ECSFramework.Utils;

using Vaerydian.Components;

namespace Vaerydian.Systems
{
    class CameraFocusSystem : EntityProcessingSystem
    {

        private ComponentMapper c_PositionMapper;
        private ComponentMapper c_CameraFocusMapper;
        private ComponentMapper c_ViewportMapper;

        private Entity c_Camera;

        public CameraFocusSystem() : base() { }

        public override void initialize()
        {
            c_PositionMapper = new ComponentMapper(new Position(), e_ECSInstance);
            c_CameraFocusMapper = new ComponentMapper(new CameraFocus(), e_ECSInstance);
            c_ViewportMapper = new ComponentMapper(new ViewPort(), e_ECSInstance);
        }

        protected override void preLoadContent(Bag<Entity> entities)
        {
            c_Camera = e_ECSInstance.TagManager.getEntityByTag("CAMERA");
        }

        protected override void process(Entity entity)
        {
            Position focusPosition = (Position) c_PositionMapper.get(entity);
            CameraFocus focus = (CameraFocus) c_CameraFocusMapper.get(entity);
            ViewPort cameraView = (ViewPort)c_ViewportMapper.get(c_Camera);

            Vector2 cPos = cameraView.getOrigin();
            Vector2 center = cPos + cameraView.getDimensions() / 2;
            Vector2 fPos = focusPosition.getPosition();
            float dist,radius;
            dist = Vector2.Distance(fPos,center);
            radius = focus.getFocusRadius();

            if (dist > radius)
            {
                Vector2 vec = Vector2.Subtract(fPos, center);
                vec.Normalize();

                cPos += Vector2.Multiply(vec, dist - radius);

                cameraView.setOrigin(cPos);
            }
        }
    }
}
