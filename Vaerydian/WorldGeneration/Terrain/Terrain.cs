using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorldGeneration.Terrain
{
    /// <summary>
    /// base terrain: ocean, land, mountain
    /// </summary>
    public enum BaseTerrainType
    {
        Land, 
        Ocean,        
        Mountain
    }

    /// <summary>
    /// type of ocean
    /// </summary>
    public enum OceanTerrainType
    {
        Ice,
        Littoral,
        Sublittoral,
        Abyssal
    }

    /// <summary>
    /// type of land
    /// </summary>
    public enum LandTerrainType
    {
        Beach,
        Desert,
        Grassland,
        Forest,
        Jungle,
        Swamp,
        Tundra,
        Arctic
    }

    /// <summary>
    /// type of mountain
    /// </summary>
    public enum MountainTerrainType
    {
        Foothill,
        Steppes,
        Cascade,
        SnowyPeak,
        Volcano
    }

    public enum WindDirection
    {
        FromNorth,
        FromEast,
        FromSouth,
        FromWest
    }

    [Serializable]
    public class Terrain
    {
        /// <summary>
        /// base terrain: ocean, land, mountain
        /// </summary>
        private BaseTerrainType t_BaseTerrainType;

        /// <summary>
        /// base terrain: ocean, land, mountain
        /// </summary>
        public BaseTerrainType BaseTerrainType
        {
            get { return t_BaseTerrainType; }
            set { t_BaseTerrainType = value; }
        }

        /// <summary>
        /// type of ocean
        /// </summary>
        private OceanTerrainType t_OceanTerrainType;
        /// <summary>
        /// type of ocean
        /// </summary>
        public OceanTerrainType OceanTerrainType
        {
            get { return t_OceanTerrainType; }
            set { t_OceanTerrainType = value; }
        }

        /// <summary>
        /// type of land
        /// </summary>
        private LandTerrainType t_LandTerrainType;
        /// <summary>
        /// type of land
        /// </summary>
        public LandTerrainType LandTerrainType
        {
            get { return t_LandTerrainType; }
            set { t_LandTerrainType = value; }
        }

        /// <summary>
        /// type of land
        /// </summary>
        private MountainTerrainType t_MountainTerrainType;
        /// <summary>
        /// type of land
        /// </summary>
        public MountainTerrainType MountainTerrainType
        {
            get { return t_MountainTerrainType; }
            set { t_MountainTerrainType = value; }
        }

        /// <summary>
        /// temperature of the terrain
        /// </summary>
        private float t_Temperature = 0f;
        /// <summary>
        /// temperature of the terrain
        /// </summary>
        public float Temperature
        {
            get { return t_Temperature; }
            set { t_Temperature = value; }
        }

        /// <summary>
        /// hight of this Terrain
        /// </summary>
        private double t_Height = 0.0;
        /// <summary>
        /// hight of this Terrain
        /// </summary>
        public double Height
        {
            get { return t_Height; }
            set { t_Height = value; }
        }



        /// <summary>
        /// amount of rainfall this terrain receives
        /// </summary>
        private float t_Rainfall = 0f;
        /// <summary>
        /// amount of rainfall this terrain receives
        /// </summary>
        public float Rainfall
        {
            get { return t_Rainfall; }
            set { t_Rainfall = value; }
        }

        /// <summary>
        /// Name of the Region
        /// </summary>
        private String t_RegionName = "";
        /// <summary>
        /// Name of the Region
        /// </summary>
        public String RegionName
        {
            get { return t_RegionName; }
            set { t_RegionName = value; }
        }

        /// <summary>
        /// direction of the wind at this cell
        /// </summary>
        private WindDirection t_WindDirection;

        /// <summary>
        /// direction of the wind at this cell
        /// </summary>
        public WindDirection WindDirection
        {
            get { return t_WindDirection; }
            set { t_WindDirection = value; }
        }
    }
}
