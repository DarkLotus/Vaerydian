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
                    return "Loading World Segment " + count + " / " + we_segmentSize * we_segmentSize;
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

        private const int we_segmentSize = 8;

        /// <summary>
        /// segements currently active
        /// </summary>
        private WorldSegment[] we_ActiveSegments = new WorldSegment[we_segmentSize * we_segmentSize];
        /// <summary>
        /// segments currently active
        /// </summary>
        public WorldSegment[] ActiveSegments
        {
            get { return we_ActiveSegments; }
            set { we_ActiveSegments = value; }
        }

        private Point we_ActiveSegmentsOffset = new Point(0, 0);

        private bool we_isLoading = false;

        private String we_LoadingMessage = "Loading...";

        private int count = 0;

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

            count = 0;

            Parallel.For(0, we_segmentSize * we_segmentSize, i =>
            {
                we_ActiveSegments[i] = (WorldSegment)loadFile(we_World.SegmentFiles[i]);
                count++;
                GC.Collect();
            });
                        
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
            int xOffset = x / 128;
            int yOffset = y / 128;
            //int xOffset = (x - we_ActiveSegmentsOffset.X * 128) / 128;
            //int yOffset = (y - we_ActiveSegmentsOffset.Y * 128) / 128;

            if ((xOffset < we_segmentSize) && (yOffset < we_segmentSize))
            {
                return we_ActiveSegments[xOffset * we_segmentSize + yOffset].Terrain[x - (xOffset * 128), y - (yOffset * 128)];
            }
            else
            {
                return new Terrain();
            }
        }


        /// <summary>
        /// update the active segments with new ones based on given position
        /// </summary>
        /// <param name="position">given position</param>
        /// <returns>the updated active segment offset</returns>
        public Point updateSegments(Point position)
        {
            int xOffset = (position.X - we_ActiveSegmentsOffset.X * 128) / 128;
            int yOffset = (position.Y - we_ActiveSegmentsOffset.Y * 128) / 128;

            /*
            if ((xOffset < 3) && (xOffset > 0) &&
                (yOffset < 3) && (xOffset > 0))
            {
                if (we_CurrentSegment == we_ActiveSegments[xOffset, yOffset])
                    return we_ActiveSegmentsOffset;
                else
                {
                    shiftSegments(position, xOffset, yOffset);
                }
            }
            */

            if (we_CurrentSegment == we_ActiveSegments[xOffset * we_segmentSize + yOffset])
            {
                return we_ActiveSegmentsOffset;
            }
            else
            {
                //calculate the difference between previous and new position to create a difference offset 
                Point diff = new Point(position.X / 128 - (we_ActiveSegmentsOffset.X + 1), position.Y / 128 - (we_ActiveSegmentsOffset.Y + 1));

                shiftSegments(diff);

                //update current segment and active segment offset
                we_CurrentSegment = we_ActiveSegments[1 * we_segmentSize + 1];
                we_ActiveSegmentsOffset.X += diff.X;
                we_ActiveSegmentsOffset.Y += diff.Y;
            }



            if (we_ActiveSegmentsOffset.X < 0)
                we_ActiveSegmentsOffset.X = 0;
            if (we_ActiveSegmentsOffset.X > we_segmentSize-1)
                we_ActiveSegmentsOffset.X = we_segmentSize-1;
            if (we_ActiveSegmentsOffset.Y < 0)
                we_ActiveSegmentsOffset.Y = 0;
            if (we_ActiveSegmentsOffset.Y > we_segmentSize-1)
                we_ActiveSegmentsOffset.Y = we_segmentSize-1;

            return we_ActiveSegmentsOffset;

        }
       
        private void shiftSegments(Point diff)
        {
            WorldSegment[] newSegments = new WorldSegment[we_segmentSize * we_segmentSize];

            //loop through all
            for (int x = 0; x < we_segmentSize; x++)
            {
                for (int y = 0; y < we_segmentSize; y++)
                {
                    //is the source segment within the current active segments
                    if ((x + diff.X) < we_segmentSize && (x + diff.X) > 0 &&
                        (y + diff.Y) < we_segmentSize && (y + diff.Y) > 0)
                    {
                        //yes, so copy it
                        newSegments[x * 3 + y] = we_ActiveSegments[(x + diff.X) * we_segmentSize + (y + diff.Y)];
                    }
                    else
                    {
                        //no, so we'll need to load it, so check to see if it is in the master list
                        if ((x + diff.X) < 8 && (x + diff.X) > 0 &&
                            (y + diff.Y) < 8 && (y + diff.Y) > 0)
                        {
                            newSegments[x * we_segmentSize + y] = (WorldSegment)loadFile(we_World.SegmentFiles[(x + diff.X) * 8 + (y + diff.Y)]);
                        }
                        else
                        {
                            //not in the master list, so create a fill-in
                            newSegments[x * we_segmentSize + y] = new WorldSegment();
                        }

                    }
                }
            }

            //perform swap
            we_ActiveSegments = newSegments;


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
                    we_World.SegmentFiles[x*8+y] = we_CurrentSegment.FileName;
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
                fs = new FileStream(filename, FileMode.Open);
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
            FileStream fs = new FileStream(filename, FileMode.Create);
            try
            {
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
