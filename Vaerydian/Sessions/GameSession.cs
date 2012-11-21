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
