using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using ECSFramework;
using ECSFramework.Utils;

using Vaerydian.Components.Graphical;
using Vaerydian.UI;

namespace Vaerydian.Systems.Update
{
    class UIUpdateSystem : EntityProcessingSystem
    {
        private ComponentMapper u_UIMapper;

        public UIUpdateSystem() { }

        public override void initialize()
        {
            u_UIMapper = new ComponentMapper(new UserInterface(), e_ECSInstance);
        }

        protected override void preLoadContent(Bag<Entity> entities)
        {
            for (int i = 0; i < entities.Size(); i++)
            {
                UserInterface ui = (UserInterface)u_UIMapper.get(entities.Get(i));
                if (!ui.UI.IsInitialized)
                    ui.UI.initialize();
            }
        }

        protected override void added(Entity entity)
        {
            UserInterface ui = (UserInterface)u_UIMapper.get(entity);
            if (!ui.UI.IsInitialized)
                ui.UI.initialize();
        }

        protected override void removed(Entity entity)
        {
            UserInterface ui = (UserInterface)u_UIMapper.get(entity);

            if (ui == null)
                return;

            if (!ui.UI.IsUnloaded)
                ui.UI.unloadContent();
        }

        protected override void process(Entity entity)
        {
            UserInterface ui = (UserInterface)u_UIMapper.get(entity);

            ui.UI.update(e_ECSInstance.ElapsedTime);
        }

    }
}
