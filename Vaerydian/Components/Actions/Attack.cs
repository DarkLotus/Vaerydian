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
    class Attack : IComponent
    {
        private static int a_TypeID;
        private int a_EntityID;

        public Attack() { }

        public Attack(Entity attacker, Entity defender, AttackType attackType)
        {
            a_Attacker = attacker;
            a_Defenter = defender;
            a_AttackType = attackType;
        }

        public int getEntityId()
        {
            return a_EntityID;
        }

        public int getTypeId()
        {
            return a_TypeID;
        }

        public void setEntityId(int entityId)
        {
            a_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            a_TypeID = typeId;
        }

        private Entity a_Attacker;

        public Entity Attacker
        {
            get { return a_Attacker; }
            set { a_Attacker = value; }
        }

        private Entity a_Defenter;

        public Entity Defender
        {
            get { return a_Defenter; }
            set { a_Defenter = value; }
        }

        private AttackType a_AttackType;

        public AttackType AttackType
        {
            get { return a_AttackType; }
            set { a_AttackType = value; }
        }


    }
}
