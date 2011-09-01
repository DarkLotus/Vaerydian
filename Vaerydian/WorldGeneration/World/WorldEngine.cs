using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace WorldGeneration.World
{

    public class WorldEngine
    {

        private WorldEngine() { } 

        private static readonly WorldEngine we_Instance = new WorldEngine();

        public static WorldEngine Instance { get{ return we_Instance; } }

        #region variables

        /// <summary>
        /// current world reference
        /// </summary>
        private World we_World = new World();
        /// <summary>
        /// current world reference
        /// </summary>
        public World World
        {
            get { return we_World; }
            set { we_World = value; }
        }

        /// <summary>
        /// world generator reference
        /// </summary>
        private WorldGenerator we_WorldGenerator = new WorldGenerator();

        /// <summary>
        /// World Generator Status Message for Loading Messages
        /// </summary>
        public String WorldGeneratorStatusMessage
        {
            get
            {
                if (we_isLoading)
                {
                    return we_LoadingMessage;
                }
                else
                {
                    return we_WorldGenerator.StatusMessage;
                }
            }
        }

        /// <summary>
        /// current segment accessed
        /// </summary>
        private WorldSegment we_CurrentSegment;

        /// <summary>
        /// segements currently active
        /// </summary>
        private WorldSegment[,] we_ActiveSegments = new WorldSegment[8, 8];
        /// <summary>
        /// segments currently active
        /// </summary>
        public WorldSegment[,] ActiveSegments
        {
            get { return we_ActiveSegments; }
            set { we_ActiveSegments = value; }
        }

        private Point we_ActiveSegmentsOffset = new Point(0, 0);

        private bool we_isLoading = false;

        private String we_LoadingMessage = "Loading...";

        #endregion 

        #region Initialization

        /// <summary>
        /// initializes the engine
        /// </summary>
        public void Initialize()
        {
            we_isLoading = true;

            //load the world
            we_World = (World) loadFile("The_World" + ".wrld");

            

            //load the active segments
            for(int i = 0; i < 8; i ++)
            {
                for (int j = 0; j < 8; j++)
                {
                    we_LoadingMessage = "Loading World Segment " + ((i * 8)+j) + " / 64";

                    we_ActiveSegments[i, j] = (WorldSegment)loadFile(we_World.SegmentFiles[i, j]);
                    GC.Collect();
                }
            }
                        
            //issue a collection
            GC.Collect();
        }

        #endregion

        #region Segment Access


        /// <summary>
        /// access terrain at given coordiantes
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <returns>terrain at given coordinate</returns>
        public Terrain getTerrain(int x, int y)
        {
            int xOffset = (x) / 128 - we_ActiveSegmentsOffset.X;
            int yOffset = (y) / 128 - we_ActiveSegmentsOffset.Y;

            if ((xOffset < 8) && (yOffset < 8))
            {
                return we_ActiveSegments[xOffset, yOffset].Terrain[x - (xOffset * 128), y - (yOffset * 128)];
            }
            else
            {
                return new Terrain();
            }
        }

       

        #endregion



        #region create, load, & save

        /// <summary>
        /// create a new world
        /// </summary>
        public void createNewWorld()
        {
            we_World = new World("The_World", 1024, 1024);

            we_WorldGenerator.generateNewWorld(0, 0, 1024, 1024, 5f, 1024, 42);


            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    //create the segment
                    we_CurrentSegment = createSegment(x, y);
                    //capture the file name
                    we_World.SegmentFiles[x, y] = we_CurrentSegment.FileName;
                    //save the segment
                    saveFile(we_CurrentSegment, we_CurrentSegment.FileName);
                }
            }

            //save world
            saveFile(we_World, we_World.Name + ".wrld");

            //loose scope of previous
            we_WorldGenerator = null;

            //issue a collection
            GC.Collect();
        }

        /// <summary>
        /// create and return a world segment
        /// </summary>
        /// <param name="i">x index</param>
        /// <param name="j">y index</param>
        /// <returns>world segment for given index</returns>
        private WorldSegment createSegment(int i, int j)
        {
            WorldSegment segment = new WorldSegment();

            for (int x = 0; x < 128; x++)
            {
                for (int y = 0; y < 128; y++)
                {
                    segment.Terrain[x, y] = we_WorldGenerator.WorldTerrainMap[x + i * 128, y + j * 128];
                }
            }

            segment.FileName = we_World.Name + "_" + i + "_" + j + ".wseg";

            return segment;
        }

        /// <summary>
        /// loads the file and returns it
        /// </summary>
        /// <param name="filename">the file to load</param>
        /// <returns>the object representing the file</returns>
        private Object loadFile(String filename)
        {
            FileStream fs = null;

            try
            {
                fs = File.Open(filename, FileMode.Open);
                return new BinaryFormatter().Deserialize(fs);
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.ToString());
                return null;
            }
            finally
            {   //close
                fs.Close();
            }
        }

        /// <summary>
        /// stores the obj in the file
        /// </summary>
        /// <param name="obj">object to store</param>
        /// <param name="filename">file to store object in</param>
        private void saveFile(Object obj, String filename)
        {
            FileStream fs = null;

            try
            {
                fs = File.Open(filename, FileMode.Create);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, obj);
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.ToString());
            }
            finally
            {
                fs.Close();
            }
        }

        #endregion

        #region update

        public void Update(GameTime gameTime)
        {
        }

        #endregion

       

    }
}
