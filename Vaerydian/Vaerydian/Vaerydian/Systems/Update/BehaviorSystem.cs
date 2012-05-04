using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using ECSFramework;
using ECSFramework.Utils;

using BehaviorLibrary;

using Vaerydian.Components;
using Vaerydian.Components.Characters;


namespace Vaerydian.Systems.Update
{
    class BehaviorSystem : EntityProcessingSystem
    {
        private ComponentMapper b_BehaviorMapper;
        private ComponentMapper b_LifeMapper;

        public BehaviorSystem() {}//: base(intervals) { }

        public override void initialize()
        {
            b_BehaviorMapper = new ComponentMapper(new AiBehavior(), e_ECSInstance);
            b_LifeMapper = new ComponentMapper(new Life(), e_ECSInstance);
        }

        protected override void preLoadContent(Bag<Entity> entities)
        {

        }

        protected override void process(Entity entity)
        {
            AiBehavior behavior = (AiBehavior)b_BehaviorMapper.get(entity);
            Life life = (Life)b_LifeMapper.get(entity);
            if (life.IsAlive)
                behavior.Behavior.Behave();
            else
            {
                if (!behavior.Behavior.IsClean)
                    behavior.Behavior.deathCleanup();
            }
        }

        
    }
}
