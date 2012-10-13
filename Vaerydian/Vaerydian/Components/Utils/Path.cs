using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using ECSFramework;

using Vaerydian.Utils;

namespace Vaerydian.Components.Utils
{

    enum PathState
    {
        Idle,
        DoPathing,
        PathFound,
        PathFailed,
        ReadyToPath,
        FollowPath
    }

    class Path : IComponent
    {
        private static int p_TypeID;

        public static int TypeID
        {
            get { return Path.p_TypeID; }
            set { Path.p_TypeID = value; }
        }

        private int p_EntityID;

        public Path() { }

        public int getEntityId()
        {
            return p_EntityID;
        }

        public int getTypeId()
        {
            return p_TypeID;
        }

        public void setEntityId(int entityId)
        {
            p_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            p_TypeID = typeId;
        }

        private Vector2 p_Start;

        public Vector2 Start
        {
            get { return p_Start; }
            set { p_Start = value; }
        }

        private Vector2 p_Finish;

        public Vector2 Finish
        {
            get { return p_Finish; }
            set { p_Finish = value; }
        }

        private GameMap p_Map;

        public GameMap Map
        {
            get { return p_Map; }
            set { p_Map = value; }
        }

        private bool p_Failed = false;

        public bool Failed
        {
            get { return p_Failed; }
            set { p_Failed = value; }
        }

        private bool p_HasRun = false;

        public bool HasRun
        {
            get { return p_HasRun; }
            set { p_HasRun = value; }
        }

        private PathState p_PathState = PathState.Idle;

        internal PathState PathState
        {
            get { return p_PathState; }
            set { p_PathState = value; }
        }

        private List<Cell> p_FoundPath;

        public List<Cell> FoundPath
        {
            get { return p_FoundPath; }
            set { p_FoundPath = value; }
        }

    }
}
