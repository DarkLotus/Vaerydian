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

    public class BinaryHeap<T>
    {
        /// <summary>
        /// data structure
        /// </summary>
        HeapCell<T>[] b_Data = new HeapCell<T>[0];

        private int b_Length;

        private int b_Size;

        public int Size
        {
            get { return b_Size; }
            set { b_Size = value; }
        }

        public BinaryHeap()
        {
            b_Size = 0;
            b_Length = 16 * 2 + 2;
            b_Data = new HeapCell<T>[b_Length];


            for (int i = 0; i < b_Length; i++)
            {
                b_Data[i] = new HeapCell<T>();
            }
        }

        public BinaryHeap(int length)
        {
            b_Size = 0;
            b_Length = length * 2 + 2;
            b_Data = new HeapCell<T>[b_Length];

            for (int i = 0; i < b_Length; i++)
            {
                b_Data[i] = new HeapCell<T>();
            }
        }

        public HeapCell<T> this[int index]
        {
            get { return b_Data[index]; }
            set { b_Data[index] = value; }
        }

        /// <summary>
        /// adds data to the heap using the given sort-value
        /// </summary>
        /// <param name="value">value used to determine proper sort</param>
        /// <param name="data">data package to store</param>
        public void add(int value, T data)
        {
            add(new HeapCell<T>(value, data));
            return;
        }

        /// <summary>
        /// adds heapcell to the heap using the given sort-value
        /// </summary>
        /// <param name="cell">heapcell to be used</param>
        private void add(HeapCell<T> cell)
        {
            b_Size++;

            if ((b_Size * 2 + 1) >= b_Length)
                grow(b_Size);

            b_Data[b_Size] = cell;

            int i = b_Size;

            //do any needed swapping
            while (i != 1)
            {
                //compare cells
                if (b_Data[i].Value <= b_Data[i / 2].Value)
                {
                    //if i is less than i/2, swap
                    HeapCell<T> temp = b_Data[i / 2];
                    b_Data[i / 2] = b_Data[i];
                    b_Data[i] = temp;
                    i = i / 2;
                }
                else//otherwise break
                    break;
            }
        }

        public HeapCell<T> removeFirst()
        {
            HeapCell<T> retVal = b_Data[1];

            //move last item to 1st position, reduce size by 1
            b_Data[1] = b_Data[b_Size];
            b_Data[b_Size] = null;
            b_Size--;

            int u, v;
            v = 1;

            //sort the heap
            while (true)
            {
                u = v;

                //if both children exist
                if ((2 * u + 1) <= b_Size)
                {
                    //select lowest child
                    if (b_Data[u].Value >= b_Data[2 * u].Value)
                        v = 2 * u;
                    if (b_Data[v].Value >= b_Data[2 * u + 1].Value)
                        v = 2 * u + 1;
                }//if only one child exists
                else if (2 * u <= b_Size)
                {
                    if (b_Data[u].Value >= b_Data[2 * u].Value)
                        v = 2 * u;
                }

                //do we need to swap or exit?
                if (u != v)
                {
                    HeapCell<T> temp = b_Data[u];
                    b_Data[u] = b_Data[v];
                    b_Data[v] = temp;
                }
                else
                {
                    break;//we've re-sorted the heap, so exit
                }
            }

            return retVal;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        private void grow(int size)
        {
            int length = size * 2 + 2;
            HeapCell<T>[] data = new HeapCell<T>[length];

            Array.Copy(b_Data, data, b_Length);

            b_Data = data;
            b_Length = length;

            return;
        }

        public void Clear()
        {
            for (int i = 0; i < b_Size; i++)
            {
                b_Data[i] = null;
            }

            b_Size = 0;
        }

        public override String ToString()
        {
            String str = "";

            for (int i = 1; i < b_Size + 1; i++)
            {
                str += b_Data[i].Value + ", ";
            }

            return str;
        }
    }
}
