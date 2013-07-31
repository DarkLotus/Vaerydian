/*
 Author:
      Thomas H. Jonell <@Net_Gnome>
 
 Copyright (c) 2013 Thomas H. Jonell

 This program is free software: you can redistribute it and/or modify
 it under the terms of the GNU Lesser General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.

 This program is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU Lesser General Public License for more details.

 You should have received a copy of the GNU Lesser General Public License
 along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

using Vaerydian.Utils;

namespace Vaerydian.Components.Actions
{
    class Damage : IComponent
    {
        private static int d_TypeID;
        private int d_EntityID;

        public Damage() { }

        public int getEntityId()
        {
            return d_EntityID;
        }

        public int getTypeId()
        {
            return d_TypeID;
        }

        public void setEntityId(int entityId)
        {
            d_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            d_TypeID = typeId;
        }

        private bool d_IsActive = true;
        /// <summary>
        /// is damage component still active
        /// </summary>
        public bool IsActive
        {
            get { return d_IsActive; }
            set { d_IsActive = value; }
        }

        private DamageClass d_DamageClass;
        /// <summary>
        /// class of damage
        /// </summary>
        public DamageClass DamageClass
        {
            get { return d_DamageClass; }
            set { d_DamageClass = value; }
        }

        private DamageType d_DamageType;
        /// <summary>
        /// type of damage
        /// </summary>
        public DamageType DamageType
        {
            get { return d_DamageType; }
            set { d_DamageType = value; }
        }

        private int d_DamageAmount = 0;
        /// <summary>
        /// amount of damage
        /// </summary>
        public int DamageAmount
        {
            get { return d_DamageAmount; }
            set { d_DamageAmount = value; }
        }

        private int d_DamageRate = 0;
        
        /// <summary>
        /// rate of damage
        /// </summary>
        public int DamageRate
        {
            get { return d_DamageRate; }
            set { d_DamageRate = value; }
        }

        private int d_TimeSinceLastDamage = 0;
        /// <summary>
        /// time since target was last damaged (only applicable for over-time damage classes)
        /// </summary>
        public int TimeSinceLastDamage
        {
            get { return d_TimeSinceLastDamage; }
            set { d_TimeSinceLastDamage = value; }
        }

        private int d_Lifetime = 0;
        /// <summary>
        /// time damage component has been alive
        /// </summary>
        public int Lifetime
        {
            get { return d_Lifetime; }
            set { d_Lifetime = value; }
        }

        private int d_Lifespan = 0;
        /// <summary>
        /// total time damage component may live
        /// </summary>
        public int Lifespan
        {
            get { return d_Lifespan; }
            set { d_Lifespan = value; }
        }

        private Entity d_Target;
        /// <summary>
        /// target of the damage
        /// </summary>
        public Entity Target
        {
            get { return d_Target; }
            set { d_Target = value; }
        }

    }
}
