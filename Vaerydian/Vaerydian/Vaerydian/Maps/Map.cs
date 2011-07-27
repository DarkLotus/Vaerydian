using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vaerydian.Maps
{
    public class Map
    {
        /// <summary>
        /// height of any given tile
        /// </summary>
        private static int m_TileHeight = 50;
        /// <summary>
        /// height of any given tile
        /// </summary>
        public int TileHeight
        {
            get { return m_TileHeight; }
            set { m_TileHeight = value; }
        }

        /// <summary>
        /// width of any given tile
        /// </summary>
        private static int m_TileWidth = 50;
        /// <summary>
        /// width of any given tile
        /// </summary>
        public int TileWidth { get { return m_TileWidth; } set { m_TileWidth = value; } }

        

        /// <summary>
        /// number of tiles in x-direction
        /// </summary>
        private static int m_xTiles = 100;
        /// <summary>
        /// number of tiles in x-direction
        /// </summary>
        public int xTiles { get { return m_xTiles; } set { m_xTiles = value; } }

        /// <summary>
        /// number of tiles in y-direction
        /// </summary>
        private static int m_yTiles = 100;
        /// <summary>
        /// number of tiles in y-direction
        /// </summary>
        public int yTiles { get { return m_yTiles; } set { m_yTiles = value; } }

        /// <summary>
        /// total pixel width of map
        /// </summary>
        private static int m_xDimension = m_xTiles * m_TileWidth;
        /// <summary>
        /// total pixel width of map
        /// </summary>
        public int xDimension { get { return m_xDimension; } set{m_xDimension = value;} }
        
        /// <summary>
        /// total pixel height of map
        /// </summary>
        private static int m_yDimension = m_yTiles * m_TileHeight;
        /// <summary>
        /// total pixel height of map
        /// </summary>
        public int yDimension { get { return m_yDimension; } set { m_yDimension = value; } }

        /// <summary>
        /// the matrix of map tiles
        /// </summary>
        private Tile[,] m_Tiles = new Tile[m_xTiles, m_yTiles];

        /// <summary>
        /// matrix of map tiles of xSize by ySize
        /// </summary>
        public Tile[,] Tiles { get { return m_Tiles; } set { m_Tiles = value; } }


        /// <summary>
        /// constructor. create a new map of xSize by ySize with tileHeight and tileWidth
        /// </summary>
        /// <param name="xSize"></param>
        /// <param name="ySize"></param>
        /// <param name="tileHeight"></param>
        /// <param name="tileWidth"></param>
        public Map(int xSize, int ySize, int tileHeight, int tileWidth)
        {
            m_TileHeight = tileHeight;
            m_TileWidth = tileWidth;
            m_xTiles = xSize;
            m_yTiles = ySize;
            m_Tiles = new Tile[m_xTiles, m_yTiles];
            m_xDimension = m_xTiles * m_TileWidth;
            m_yDimension = m_yTiles * m_TileHeight;
        }




    }
}
