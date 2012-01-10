using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework.Utils;

namespace ECSFramework
{
    /// <summary>
    /// controls access to ongoing ECS managers within the current ECS Instance
    /// </summary>
    public class ECSInstance
    {

        private EntityManager e_EntityManager;

        public EntityManager EntityManager
        {
            get { return e_EntityManager; }
            set { e_EntityManager = value; }
        }
       
        private SystemManager e_SystemManager;

        public SystemManager SystemManager
        {
            get { return e_SystemManager; }
            set { e_SystemManager = value; }
        }
        
        private GroupManager e_GroupManager;

        public GroupManager GroupManager
        {
            get { return e_GroupManager; }
            set { e_GroupManager = value; }
        }
        
        private TagManager e_TagManager;

        public TagManager TagManager
        {
            get { return e_TagManager; }
            set { e_TagManager = value; }
        }
        
        private ComponentManager e_ComponentManager;

        public ComponentManager ComponentManager
        {
            get { return e_ComponentManager; }
            set { e_ComponentManager = value; }
        }

        private int e_TotalTime;

        private Bag<Entity> e_Refreshing;
        private Bag<Entity> e_Deleting;

        public ECSInstance()
        {
            e_EntityManager = new EntityManager(this);
            e_SystemManager = new SystemManager(this);
            e_GroupManager = new GroupManager(this);
            e_TagManager = new TagManager(this);
            e_ComponentManager = new ComponentManager(this);
        }

        /// <summary>
        /// create an entity
        /// </summary>
        /// <returns>the new entity</returns>
        public Entity create()
        {
            return e_EntityManager.create();
        }

        /// <summary>
        /// delete the given entity
        /// </summary>
        /// <param name="e">the entity to be deleted</param>
        public void delete(Entity e)
        {
            if (!e_Deleting.Contains(e))
                e_Deleting.Add(e);
        }

        //refresh the given entity
        public void refresh(Entity e)
        {
            e_Refreshing.Add(e);
        }

        //refresh the given entity with all systems
        private void refreshEntity(Entity e)
        {
            e_SystemManager.refresh(e);
        }

        /// <summary>
        /// resolves and updates any entity refrences across the ECS Instance
        /// </summary>
        public void resolveAndUpdate()
        {
            //ensure any entities that need to be refreshed, are refreshed
            if (e_Refreshing.Size() >= 0)
            {
                for (int i = 0; i < e_Refreshing.Size(); i++)
                {
                    refreshEntity(e_Refreshing.Get(i));
                }
            }
            
            //ensure any entities that need to be deleted, are deleted
            if (e_Deleting.Size() >= 0)
            {
                for (int i = 0; i < e_Deleting.Size(); i++)
                {
                    e_EntityManager.delete(e_Deleting.Get(i));
                    e_GroupManager.delete(e_Deleting.Get(i));
                    e_TagManager.delete(e_Deleting.Get(i));
                }
            }

            
        }


    }
}
