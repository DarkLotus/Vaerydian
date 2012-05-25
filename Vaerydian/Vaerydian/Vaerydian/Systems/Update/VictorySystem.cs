using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;
using ECSFramework.Utils;

using Vaerydian.Components.Characters;
using Vaerydian.Components.Utils;
using Vaerydian.Characters.Experience;

namespace Vaerydian.Systems.Update
{
    class VictorySystem : EntityProcessingSystem
    {
        private ComponentMapper v_VictoryMapper;
        private ComponentMapper v_KnowledgeMapper;
        private ComponentMapper v_AggrivationMapper;

        public VictorySystem(){ }
        
        public override void initialize()
        {
            v_KnowledgeMapper = new ComponentMapper(new Knowledges(), e_ECSInstance);
            v_VictoryMapper = new ComponentMapper(new Victory(), e_ECSInstance);
            v_AggrivationMapper = new ComponentMapper(new Aggrivation(), e_ECSInstance);
            
        }
        
        protected override void preLoadContent(Bag<Entity> entities)
        {
            
        }
                
        protected override void process(Entity entity)
        {
            Victory victory = (Victory)v_VictoryMapper.get(entity);
            Aggrivation aggro = (Aggrivation)v_AggrivationMapper.get(entity);


            Knowledges awarder = (Knowledges)v_KnowledgeMapper.get(victory.Awarder);
            Knowledges receiver = (Knowledges)v_KnowledgeMapper.get(victory.Receiver);

            //if either is not available, don't continue
            if (awarder == null || receiver == null)
                return;


            
        }

    }
}
