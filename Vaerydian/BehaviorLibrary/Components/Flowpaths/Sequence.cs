using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorLibrary.Components.Flowpaths
{
    public class Sequence : BehaviorComponent
    {

        protected BehaviorComponent[] s_Behaviors;

        private short sequence = 0;

        private short seqLength = 0;

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="behaviors"></param>
        public Sequence(params BehaviorComponent[] behaviors)
        {
            s_Behaviors = behaviors;
            seqLength = (short) s_Behaviors.Length;
        }


        public BehaviorReturnCode Behave()
        {
            //while you can go through them, do so
            while (sequence < seqLength)
            {
                switch (s_Behaviors[sequence].Behave())
                {
                    case BehaviorReturnCode.Failure:
                        sequence = 0;
                        return BehaviorReturnCode.Failure;
                    case BehaviorReturnCode.Success:
                        sequence++;
                        return BehaviorReturnCode.Running;
                    case BehaviorReturnCode.Running:
                        return BehaviorReturnCode.Running;
                }

            }

            sequence = 0;
            return BehaviorReturnCode.Success;

        }

    }
}
