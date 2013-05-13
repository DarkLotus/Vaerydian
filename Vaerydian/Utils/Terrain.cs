using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;



namespace Vaerydian.Utils
{
	public enum TerrainType{
		NOTHING,
		BOUNDARY,
		FLOOR,
		WALL,
		DECORATION,
		TRANSITION,
		TRIGGER
	}

	public struct TerrainDef{
		public string Name;
		public short ID;
		public string Texture;
		public Point TextureOffset;
		public Color Color;
		public bool Passible;
		public short Effect;
		public TerrainType TerrainType;
	}

    /// <summary>
    /// holds information about a cell's terrain
    /// </summary>
    public class Terrain
    {
        private short t_TerrainType = 0;
        /// <summary>
        /// type of terrain
        /// </summary>
        public short TerrainType
        {
            get { return t_TerrainType; }
            set { t_TerrainType = value; }
        }

		private TerrainDef t_TerrainDef;

		public TerrainDef TerrainDef {
			get {
				return t_TerrainDef;
			}
			set {
				t_TerrainDef = value;
			}
		}

        private bool t_IsBlocking;
        /// <summary>
        /// does the terrain block movement
        /// </summary>
        public bool IsBlocking
        {
            get { return t_IsBlocking; }
            set { t_IsBlocking = value; }
        }

        private short t_ObjectType;
        /// <summary>
        /// type of object on this terrain
        /// </summary>
        public short ObjectType
        {
            get { return t_ObjectType; }
            set { t_ObjectType = value; }
        }

        private float t_Height;
        /// <summary>
        /// height of terrain
        /// </summary>
        public float Height
        {
            get { return t_Height; }
            set { t_Height = value; }
        }

        private float t_Rainfall;
        /// <summary>
        /// rainfall in this terrain
        /// </summary>
        public float Rainfall
        {
            get { return t_Rainfall; }
            set { t_Rainfall = value; }
        }

        private float t_Temperature;
        /// <summary>
        /// temperature in this terrain
        /// </summary>
        public float Temperature
        {
            get { return t_Temperature; }
            set { t_Temperature = value; }
        }

        private float t_WindX;
        /// <summary>
        /// wind in x-direction
        /// </summary>
        public float WindX
        {
            get { return t_WindX; }
            set { t_WindX = value; }
        }

        private float t_WindY;
        /// <summary>
        /// wind in y-direction
        /// </summary>
        public float WindY
        {
            get { return t_WindY; }
            set { t_WindY = value; }
        }

        private float t_Moisture;
        /// <summary>
        /// moisture
        /// </summary>
        public float Moisture
        {
            get { return t_Moisture; }
            set { t_Moisture = value; }
        }

        private float t_Variation = 1f;

        public float Variation
        {
            get { return t_Variation; }
            set { t_Variation = value; }
        }
    }
}
