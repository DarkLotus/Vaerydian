using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using ECSFramework;
using ECSFramework.Utils;

using BehaviorLibrary;

using Vaerydian.Components;


namespace Vaerydian.Systems.Update
{
    class BehaviorSystem : EntityProcessingSystem
    {
        private ComponentMapper b_BehaviorMapper;

        public BehaviorSystem() {}//: base(intervals) { }

        public override void initialize()
        {
            b_BehaviorMapper = new ComponentMapper(new AiBehavior(), e_ECSInstance);
        }

        protected override void preLoadContent(Bag<Entity> entities)
        {

        }

        protected override void process(Entity entity)
        {
            AiBehavior behavior = (AiBehavior)b_BehaviorMapper.get(entity);
            behavior.getBehavior().Behave();
        }

        
    }
}
