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
    public class HeapCell<T>
    {
        private T h_Data;
        /// <summary>
        /// data cell contains
        /// </summary>
        public T Data
        {
            get { return h_Data; }
            set { h_Data = value; }
        }

        private int h_Value;
        /// <summary>
        /// indexing value
        /// </summary>
        public int Value
        {
            get { return h_Value; }
            set { h_Value = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="data"></param>
        public HeapCell(int value, T data)
        {
            h_Value = value;
            h_Data = data;
        }

        public HeapCell() { }
    }
}
