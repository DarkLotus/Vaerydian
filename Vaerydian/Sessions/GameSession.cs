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

using Vaerydian.Characters;
using Vaerydian.Maps;
using Vaerydian.Components.Utils;

namespace Vaerydian.Sessions
{
    /// <summary>
    /// this singleton manages and provides access to the ongoing game session
    /// </summary>
    static class GameSession
    {

        private static String g_GameVersion = "Alpha 0.0.5.0";

        public static String GameVersion
        {
            get { return g_GameVersion; }
            set { g_GameVersion = value; }
        }

        private static GameMap g_WorldMap;

        public static GameMap WorldMap
        {
            get { return GameSession.g_WorldMap; }
            set { GameSession.g_WorldMap = value; }
        }

        private static Stack<MapState> g_MapStack = new Stack<MapState>();

        public static Stack<MapState> MapStack
        {
            get { return GameSession.g_MapStack; }
            set { GameSession.g_MapStack = value; }
        }


        private static PlayerState g_PlayerState;

        public static PlayerState PlayerState
        {
            get { return GameSession.g_PlayerState;}
            set { GameSession.g_PlayerState = value; }
        }

    }
}
