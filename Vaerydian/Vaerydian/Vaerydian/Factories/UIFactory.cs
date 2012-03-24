using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECSFramework;

using Vaerydian.UI;
using Vaerydian.Components.Graphical;
using Microsoft.Xna.Framework;

namespace Vaerydian.Factories
{
    public class UIFactory
    {
        private ECSInstance u_EcsInstance;
        private static GameContainer u_Container;
        private Random rand = new Random();

        public UIFactory(ECSInstance ecsInstance, GameContainer container)
        {
            u_EcsInstance = ecsInstance;
            u_Container = container;
        }

        public UIFactory(ECSInstance ecsInstance) 
        {
            u_EcsInstance = ecsInstance;
        }

        public void createTimedDialogWindow(Entity caller, String dialog, Vector2 origin, String name, int duration)
        {
            Entity e = u_EcsInstance.create();

            TimedDialogWindow window = new TimedDialogWindow(caller, dialog, origin, name, duration);

            window.ECSInstance = u_EcsInstance;
            window.Owner = e;

            UserInterface ui = new UserInterface(window);

            u_EcsInstance.EntityManager.addComponent(e, ui);

            u_EcsInstance.refresh(e);
        }
    }
}
