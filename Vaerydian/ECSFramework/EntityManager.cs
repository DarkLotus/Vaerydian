using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework.Utils;

namespace ECSFramework
{
    public class EntityManager
    {

        private ECSInstance e_ECSInstance;

        private int e_NextId = 0;

        private Bag<Entity> e_DeletedEntities;
        private Bag<Entity> e_ActiveEntities;


        public EntityManager(ECSInstance ecsInstance)
        {
            this.e_ECSInstance = ecsInstance;
            this.e_DeletedEntities = new Bag<Entity>(16);
            this.e_ActiveEntities = new Bag<Entity>(64);
        }

        /// <summary>
        /// create a new entity attempting to reuse a previously deleted one first
        /// </summary>
        /// <returns>the new entity</returns>
        public Entity create()
        {
            Entity e = e_DeletedEntities.RemoveLast();
            if (e == null)
                e = new Entity(e_NextId++);

            e_ActiveEntities.Add(e);

            return e;
        }

        /// <summary>
        /// deletes an entity
        /// </summary>
        /// <param name="e">entity to be deleted</param>
        public void delete(Entity e)
        {
            //replace entry with a null, but leave the spot alone
            e_ActiveEntities.Set(e.Id, null);
            //refresh as appropriate
            e_ECSInstance.refresh(e);
            //ensure the components are removed
            e_ECSInstance.ComponentManager.removeComponents(e);
            //add it to the deleted entities bag for reuse
            e_DeletedEntities.Add(e);
        }

    }
}
