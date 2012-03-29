using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vaerydian.UI
{
    class HotBar : IUserInterface
    {

        public HotBar(HotBarItem[] items)
        {

        }

        public ECSFramework.Entity Owner
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public ECSFramework.Entity Caller
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public ECSFramework.ECSInstance ECSInstance
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public GameContainer GameContainer
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsInitialized
        {
            get { throw new NotImplementedException(); }
        }

        public void initialize()
        {
            throw new NotImplementedException();
        }

        public bool IsLoaded
        {
            get { throw new NotImplementedException(); }
        }

        public void loadContent()
        {
            throw new NotImplementedException();
        }

        public bool IsUnloaded
        {
            get { throw new NotImplementedException(); }
        }

        public void unloadContent()
        {
            throw new NotImplementedException();
        }

        public void update(int elapsedTime)
        {
            throw new NotImplementedException();
        }

        public void draw(int elapsedTime)
        {
            throw new NotImplementedException();
        }
    }
}
