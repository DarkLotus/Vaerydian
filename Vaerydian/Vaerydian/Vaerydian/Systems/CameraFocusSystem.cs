using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using ECSFramework;

using Vaerydian.Components;

namespace Vaerydian.Systems
{
    class CameraFocusSystem : EntityProcessingSystem
    {

        private ComponentMapper c_PositionMapper;
        private ComponentMapper c_CameraFocusMapper;
        private ComponentMapper c_VelocityMapper;
        private ComponentMapper c_ViewportMapper;

        public CameraFocusSystem() : base() { }

        public override void initialize()
        {
            c_PositionMapper = new ComponentMapper(new Position(), e_ECSInstance);
            c_CameraFocusMapper = new ComponentMapper(new CameraFocus(), e_ECSInstance);
            c_VelocityMapper = new ComponentMapper(new Velocity(), e_ECSInstance);
            c_ViewportMapper = new ComponentMapper(new ViewPort(), e_ECSInstance);
        }

        protected override void process(Entity entity)
        {
            Position focusPosition = (Position) c_PositionMapper.get(entity);
            CameraFocus focus = (CameraFocus) c_CameraFocusMapper.get(entity);
            
            Entity camera = e_ECSInstance.TagManager.getEntityByTag("CAMERA");

            ViewPort cameraView = (ViewPort) c_ViewportMapper.get(camera);
            Velocity cameraVelocity = (Velocity) c_VelocityMapper.get(camera);

            Vector2 cPos = cameraView.getOrigin();
            Vector2 fPos = focusPosition.getPosition();
            float dist,radius;
            dist = Vector2.Distance(fPos,cPos);
            radius = focus.getFocusRadius();

            if (dist > radius)
            {
                Vector2 vec = new Vector2(fPos.X - cPos.X, fPos.Y - cPos.Y);
                vec.Normalize();
                //cPos += Vector2.Multiply(vec,cameraVelocity.getVelocity()*1.414f);
                cPos += Vector2.Multiply(vec, dist - radius);

                cameraView.setOrigin(cPos);
            }
        }
    }
}
