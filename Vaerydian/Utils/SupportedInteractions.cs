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
    
    
    public struct SupportedInteractions
    {
        public bool PROJECTILE_COLLIDABLE;
        public bool DESTROYABLE;
        public bool ATTACKABLE;
        public bool MELEE_ACTIONABLE;
        public bool AWARDS_VICTORY;
        public bool MAY_RECEIVE_VICTORY;
        public bool CAUSES_ADVANCEMENT;
        public bool MAY_ADVANCE;
    }
}
