using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

using Vaerydian.Characters;

namespace Vaerydian.Components.Characters
{
    class Factions : IComponent
    {
        private static int f_TypeID;
        private int f_EntityID;

        public Factions() { }

        public int getEntityId()
        {
            return f_EntityID;
        }

        public int getTypeId()
        {
            return f_TypeID;
        }

        public void setEntityId(int entityId)
        {
            f_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            f_TypeID = typeId;
        }

        private Faction f_OwnerFaction;

        public Faction OwnerFaction
        {
            get { return f_OwnerFaction; }
            set { f_OwnerFaction = value; }
        }

        private Dictionary<FactionType, Faction> f_KnownFactions = new Dictionary<FactionType, Faction>();
        /// <summary>
        /// factions known to entity
        /// </summary>
        public Dictionary<FactionType, Faction> KnownFactions
        {
            get { return f_KnownFactions; }
            set { f_KnownFactions = value; }
        }
    }
}
