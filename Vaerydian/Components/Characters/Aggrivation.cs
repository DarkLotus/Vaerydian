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

namespace Vaerydian.Components.Characters
{
    class Aggrivation : IComponent
    {
        private static int a_TypeID;

        public static int TypeID
        {
            get { return Aggrivation.a_TypeID; }
            set { Aggrivation.a_TypeID = value; }
        }
        private int a_EntityID;

        public Aggrivation() { }

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

        private List<Entity> a_HateList = new List<Entity>();

        public List<Entity> HateList
        {
            get { return a_HateList; }
            set { a_HateList = value; }
        }

        private Entity a_Target;

        public Entity Target
        {
            get { return a_Target; }
            set { a_Target = value; }
        }
    }
}
