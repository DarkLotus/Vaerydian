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
