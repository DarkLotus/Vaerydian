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
    public static class ObjectType
    {
        public const short NOTHING = 0;
        public const short TRANSITION = 1;
        public const short TRIGGER = 2;
        public const short DRAMA = 3;
		public const short TREE = 4;
		public const short HOLE = 5;
		public const short CAVE = 6;
		public const short STAIRS_UP = 7;
		public const short STAIRS_DOWN = 8;
    }
}
