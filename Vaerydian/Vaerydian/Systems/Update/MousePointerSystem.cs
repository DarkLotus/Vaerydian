using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using ECSFramework;


using Vaerydian.Components;

using Glimpse.Input;
using Vaerydian.Components.Spatials;
using Vaerydian.Components.Graphical;

namespace Vaerydian.Systems.Update
{
    class MousePointerSystem : EntityProcessingSystem
    {

        private ComponentMapper m_PositionMapper;
        private ComponentMapper m_ViewPortMapper;

        private Entity m_Camera;

        public MousePointerSystem() : base() { }

        public override void initialize()
        {
            m_PositionMapper = new ComponentMapper(new Position(), e_ECSInstance);
            m_ViewPortMapper = new ComponentMapper(new ViewPort(), e_ECSInstance);
        }

        protected  override void preLoadContent(Bag<Entity> entities)
        {
            m_Camera = e_ECSInstance.TagManager.getEntityByTag("CAMERA");
        }

        protected override void process(Entity entity)
        {
            Position pos = (Position) m_PositionMapper.get(entity);

            ViewPort viewPort = (ViewPort)m_ViewPortMapper.get(m_Camera);
            Vector2 center = viewPort.getDimensions() / 2;
            Vector2 origin = viewPort.getOrigin();

            pos.Pos = InputManager.getMousePositionVector()+origin;
        }
    }
}
