using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Vaerydian.Components.Spatials;

namespace Vaerydian.Maps
{
    class MapState
    {

        private short m_MapType;

        public short MapType
        {
            get { return m_MapType; }
            set { m_MapType = value; }
        }

        private int m_SkillLevel;

        public int SkillLevel
        {
            get { return m_SkillLevel; }
            set { m_SkillLevel = value; }
        }

        private string m_LocationName;

        public string LocationName
        {
            get { return m_LocationName; }
            set { m_LocationName = value; }
        }

        private int m_Seed;

        public int Seed
        {
            get { return m_Seed; }
            set { m_Seed = value; }
        }

        private Position m_LastPlayerPosition;

        public Position LastPlayerPosition
        {
            get { return m_LastPlayerPosition; }
            set { m_LastPlayerPosition = value; }
        }


    }
}
