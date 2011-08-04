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
    class GameSession
    {
        /// <summary>
        /// singleton
        /// </summary>
        private GameSession() { }
        /// <summary>
        /// private singleton access variable
        /// </summary>
        private static readonly GameSession gs_Instance = new GameSession();
        /// <summary>
        /// singleton access variable
        /// </summary>
        public static GameSession Instance { get { return gs_Instance; } }

        /// <summary>
        /// player's character
        /// </summary>
        private PlayerCharacter gs_PlayerCharacter;

        /// <summary>
        /// player's character
        /// </summary>
        internal PlayerCharacter PlayerCharacter
        {
            get { return gs_PlayerCharacter; }
            set { gs_PlayerCharacter = value; }
        }

    }
}
