using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vaerydian.Utils
{

    //TODO: test binary heap
    public class BinaryHeap<T>
    {
        /// <summary>
        /// data structure
        /// </summary>
        HeapCell<T>[] b_Data;

        private int b_Length;

        private int b_Size;


        public BinaryHeap(int size)
        {
            b_Size = size;
            b_Length = size * 2 + 2;
            b_Data = new HeapCell<T>[b_Length];
        }

        /// <summary>
        /// adds data to the heap using the given sort-value
        /// </summary>
        /// <param name="value">value used to determine proper sort</param>
        /// <param name="data">data package to store</param>
        public void add(int value, T data)
        {
            add(new HeapCell<T>(value,data));
            return;
        }

        /// <summary>
        /// adds heapcell to the heap using the given sort-value
        /// </summary>
        /// <param name="cell">heapcell to be used</param>
        private void add(HeapCell<T> cell)
        {
            b_Size++;

            if ((b_Size*2+1) >= b_Length)
                grow(b_Size);

            b_Data[b_Length - 1] = cell;

            int i = b_Length;

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
            return;
        }

        public void remove(int index)
        {
            //TODO: add heapcell remove
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
    }
}
