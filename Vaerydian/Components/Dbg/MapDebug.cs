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


namespace Vaerydian.Components.Dbg
{
    public class MapDebug : IComponent
    {

        private static int m_TypeID;
        private int m_EntityID;

        private List<Cell> m_Path = new List<Cell>();

        public List<Cell> Path
        {
            get { return m_Path; }
            set { m_Path = value; }
        }

        private List<Cell> m_Blocking = new List<Cell>();

        public List<Cell> Blocking
        {
            get { return m_Blocking; }
            set { m_Blocking = value; }
        }

        /*private List<Cell> m_OpenSet = new List<Cell>();

        public List<Cell> OpenSet
        {
            get { return m_OpenSet; }
            set { m_OpenSet = value; }
        }*/

        private BinaryHeap<Cell> m_OpenSet = new BinaryHeap<Cell>();

        public BinaryHeap<Cell> OpenSet
        {
            get { return m_OpenSet; }
            set { m_OpenSet = value; }
        }

        private List<Cell> m_ClosedSet = new List<Cell>();

        public List<Cell> ClosedSet
        {
            get { return m_ClosedSet; }
            set { m_ClosedSet = value; }
        }

        public MapDebug() { }

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
    }
}
