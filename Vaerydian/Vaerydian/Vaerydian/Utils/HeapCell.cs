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
    }
}
