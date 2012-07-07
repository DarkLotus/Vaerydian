using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vaerydian.Utils
{
    /*
    public enum InteractionTypes
    {
        Projectile_Collidable,
        Destroyable,
        Damageable
    }*/
    
    
    public class SupportedInteractions
    {
        public SupportedInteractions() { }

        public bool PROJECTILE_COLLIDABLE = false;

        public bool DESTROYABLE = false;

        public bool ATTACKABLE = false;

        public bool MELEE_ACTIONABLE = false;

        public bool AWARDS_VICTORY = false;

        public bool MAY_RECEIVE_VICTORY = false;

        public bool CAUSES_ADVANCEMENT = false;

        public bool MAY_ADVANCE = false;
    }
}
