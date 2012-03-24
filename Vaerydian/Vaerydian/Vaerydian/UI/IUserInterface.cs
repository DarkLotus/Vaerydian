using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using ECSFramework;

namespace Vaerydian.UI
{
    public interface IUserInterface
    {
        Entity Owner { get; set; }

        Entity Caller { get; set; }

        ECSInstance ECSInstance { get; set; }

        GameContainer GameContainer { get; set; }

        bool IsInitialized { get;}

        void initialize();

        bool IsLoaded { get;}

        void loadContent();

        bool IsUnloaded { get;}

        void unloadContent();

        void update(int elapsedTime);

        void draw(int elapsedTime);

    }
}
