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

using Microsoft.Xna.Framework;

using Vaerydian.Utils;

namespace Vaerydian.Components.Actions
{
    public class MeleeAction : IComponent
    {
        private static int m_TypeID;
        private int m_EntityID;

        public MeleeAction() { }

        public int getEntityId()
        {
            return m_EntityID;
        }

        public int getTypeId()
        {
            return m_TypeID;
        }

        public void setEntityId(int entityId)
        {
            m_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            m_TypeID = typeId;
        }

        
        private Entity m_Owner;
        /// <summary>
        /// owner of this action
        /// </summary>
        public Entity Owner
        {
            get { return m_Owner; }
            set { m_Owner = value; }
        }

        private int m_Lifetime = 0;
        /// <summary>
        /// the max lifetime of this action
        /// </summary>
        public int Lifetime
        {
            get { return m_Lifetime; }
            set { m_Lifetime = value; }
        }
        
        private int m_ElapsedTime = 0;
        /// <summary>
        /// the current elapsed time of this action
        /// </summary>
        public int ElapsedTime
        {
            get { return m_ElapsedTime; }
            set { m_ElapsedTime = value; }
        }

        private float m_Range = 0f;

        /// <summary>
        /// range of melee action
        /// </summary>
        public float Range
        {
            get { return m_Range; }
            set { m_Range = value; }
        }

        private SpriteAnimation m_Animation;
        /// <summary>
        /// the animation for this action
        /// </summary>
        public SpriteAnimation Animation
        {
            get { return m_Animation; }
            set { m_Animation = value; }
        }

        private float m_ArcDegrees = 90;

        /// <summary>
        /// swing arc in degrees
        /// </summary>
        public float ArcDegrees
        {
            get { return m_ArcDegrees; }
            set { m_ArcDegrees = value; }
        }

        private List<Entity> m_HitByAction = new List<Entity>();

        /// <summary>
        /// entities that have been hit by this action
        /// </summary>
        public List<Entity> HitByAction
        {
            get { return m_HitByAction; }
            set { m_HitByAction = value; }
        }
        
    }
}
