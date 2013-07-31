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

using Microsoft.Xna.Framework;

using ECSFramework;

using Vaerydian.Utils;

namespace Vaerydian.Components.Utils
{

    enum PathState
    {
        Idle,
        DoPathing,
        PathFound,
        PathFailed
    }

    class Path : IComponent
    {
        private static int p_TypeID;

        public static int TypeID
        {
            get { return Path.p_TypeID; }
            set { Path.p_TypeID = value; }
        }

        private int p_EntityID;

        public Path() { }

        public int getEntityId()
        {
            return p_EntityID;
        }

        public int getTypeId()
        {
            return p_TypeID;
        }

        public void setEntityId(int entityId)
        {
            p_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            p_TypeID = typeId;
        }

        private Vector2 p_Start;

        public Vector2 Start
        {
            get { return p_Start; }
            set { p_Start = value; }
        }

        private Vector2 p_Finish;

        public Vector2 Finish
        {
            get { return p_Finish; }
            set { p_Finish = value; }
        }

        private GameMap p_Map;

        public GameMap Map
        {
            get { return p_Map; }
            set { p_Map = value; }
        }

        private bool p_Failed = false;

        public bool Failed
        {
            get { return p_Failed; }
            set { p_Failed = value; }
        }

        private bool p_HasRun = false;

        public bool HasRun
        {
            get { return p_HasRun; }
            set { p_HasRun = value; }
        }

        private PathState p_PathState = PathState.Idle;

        internal PathState PathState
        {
            get { return p_PathState; }
            set { p_PathState = value; }
        }

        private List<Cell> p_FoundPath;

        public List<Cell> FoundPath
        {
            get { return p_FoundPath; }
            set { p_FoundPath = value; }
        }

        private AStarPathing p_Pathing;

        public AStarPathing Pathing
        {
            get { return p_Pathing; }
            set { p_Pathing = value; }
        }

    }
}
