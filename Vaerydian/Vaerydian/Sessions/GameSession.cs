using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vaerydian.Characters;
using Vaerydian.Maps;

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

        private static Stack<MapDef> g_MapDefStack;


        private static string PLAYER_PLACEHOLDER;

    }
}
