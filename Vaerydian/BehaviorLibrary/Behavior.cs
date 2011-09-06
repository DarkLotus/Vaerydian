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

        private BehaviorComponent b_BehaviorComponent;

        private BehaviorReturnCode b_ReturnCode;

        public BehaviorReturnCode ReturnCode
        {
            get { return b_ReturnCode; }
            set { b_ReturnCode = value; }
        }

        public Behavior(BehaviorComponent behaviorComponent)
        {
            b_BehaviorComponent = behaviorComponent;
        }

        public void run()
        {
            try
            {
                b_ReturnCode = b_BehaviorComponent.Behave();
            }
            catch (Exception)
            {
            }
        }

    }
}
