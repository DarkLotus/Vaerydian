using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;
using ECSFramework.Utils;

using Vaerydian.Components.Characters;

namespace Vaerydian.Systems.Update
{
    class VictorySystem : EntityProcessingSystem
    {
        private ComponentMapper v_VictoryMapper;
        private ComponentMapper v_ExperienceMapper;

        public VictorySystem(){ }
        
        public override void initialize()
        {
            v_ExperienceMapper = new ComponentMapper(new Experiences(), e_ECSInstance);
            v_VictoryMapper = new ComponentMapper(new Victory(), e_ECSInstance);
        }
        
        protected override void preLoadContent(Bag<Entity> entities)
        {
            
        }
                
        protected override void process(Entity entity)
        {
            Victory victory = (Victory)v_VictoryMapper.get(entity);


        }

    }
}
