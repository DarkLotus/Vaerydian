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

        /// <summary>
        /// player's character
        /// </summary>
        private static Character gs_PlayerCharacter;

        /// <summary>
        /// player's character
        /// </summary>
        public static Character PlayerCharacter
        {
            get { return gs_PlayerCharacter; }
            set { gs_PlayerCharacter = value; }
        }

        private static String gs_GameVersion = "Alpha 0.0.2.0";

        public static String GameVersion
        {
            get { return gs_GameVersion; }
            set { gs_GameVersion = value; }
        }



    }
}
