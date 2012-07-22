using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vaerydian.Characters;

namespace Vaerydian.Sessions
{
    /// <summary>
    /// this singleton manages and provides access to the ongoing game session
    /// </summary>
    static class GameSession
    {

        private static String gs_GameVersion = "Alpha 0.0.3.0";

        public static String GameVersion
        {
            get { return gs_GameVersion; }
            set { gs_GameVersion = value; }
        }



    }
}
