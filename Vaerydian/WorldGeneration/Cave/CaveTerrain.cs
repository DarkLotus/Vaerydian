using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorldGeneration.Cave
{
    public enum CaveType
    {
        Wall,
        Floor
    }

    public class CaveTerrain
    {
        /// <summary>
        /// type of cave this terrain represents
        /// </summary>
        private CaveType c_BaseCaveType;
        /// <summary>
        /// type of cave this terrain represents
        /// </summary>
        public CaveType BaseCaveType
        {
            get { return c_BaseCaveType; }
            set { c_BaseCaveType = value; }
        }
    }
}
