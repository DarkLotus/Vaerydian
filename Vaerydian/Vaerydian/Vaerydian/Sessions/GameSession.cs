using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public GameSession Instance { get { return gs_Instance; } }

    }
}
