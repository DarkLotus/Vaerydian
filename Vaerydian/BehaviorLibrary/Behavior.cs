using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using BehaviorLibrary.Components;
using BehaviorLibrary.Components.Composites;

namespace BehaviorLibrary
{
    public enum BehaviorReturnCode
    {
        Failure,
        Success,
        Running
    }

    /// <summary>
    /// 
    /// </summary>
    public class Behavior
    {

        private RootSelector b_Root;

        private BehaviorReturnCode b_ReturnCode;

        public BehaviorReturnCode ReturnCode
        {
            get { return b_ReturnCode; }
            set { b_ReturnCode = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        public Behavior(RootSelector root)
        {
            b_Root = root;
        }

        /// <summary>
        /// 
        /// </summary>
        public void run()
        {
            try
            {
                switch (b_Root.Behave())
                {
                    case BehaviorReturnCode.Failure:
                        ReturnCode = BehaviorReturnCode.Failure;
                        return;
                    case BehaviorReturnCode.Success:
                        ReturnCode = BehaviorReturnCode.Success;
                        return;
                    case BehaviorReturnCode.Running:
                        ReturnCode = BehaviorReturnCode.Running;
                        return;
                }
            }
            catch (Exception)
            {
                ReturnCode = BehaviorReturnCode.Failure;
                return;
            }

            ReturnCode = BehaviorReturnCode.Running;
            return;
        }

    }
}
