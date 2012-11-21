using System;

namespace Vaerydian
{

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (VaerydianGame game = new VaerydianGame())
            {
                game.Run();
            }
        }
    }

}

