using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorLibrary.Components.Composites
{
    public class Selector : BehaviorComponent
    {

        protected BehaviorComponent[] s_Behaviors;

        private short selections = 0;

        private short selLength = 0;

        /// <summary>
        /// Selects among the given behavior components
        /// Performs an OR-Like behavior and will "fail-over" to each successive component until Success is reached or Failure is certain
        /// -Returns Success if a behavior component returns Success
        /// -Returns Running if a behavior component returns Failure or Running or an error occurs
        /// -Returns Failure if all behavior components returned Failure
        /// </summary>
        /// <param name="behaviors">one to many behavior components</param>
        public Selector(params BehaviorComponent[] behaviors)
        {
            s_Behaviors = behaviors;
            selLength = (short)s_Behaviors.Length;
        }

        /// <summary>
        /// performs the given behavior
        /// </summary>
        /// <returns>the behaviors return code</returns>
        public BehaviorReturnCode Behave()
        {
            while (selections < selLength)
            {
                try
                {
                    switch (s_Behaviors[selections].Behave())
                    {
                        case BehaviorReturnCode.Failure:
                            selections++;
                            this.ReturnCode = BehaviorReturnCode.Running;
                            return BehaviorReturnCode.Running;
                        case BehaviorReturnCode.Success:
                            selections = 0;
                            this.ReturnCode = BehaviorReturnCode.Success;
                            return BehaviorReturnCode.Success;
                        case BehaviorReturnCode.Running:
                            this.ReturnCode = BehaviorReturnCode.Running;
                            return BehaviorReturnCode.Running;
                    }
                }
                catch (Exception)
                {
                    selections++;
                    this.ReturnCode = BehaviorReturnCode.Running;
                    return BehaviorReturnCode.Running;
                }

                selections = 0;
                return BehaviorReturnCode.Failure;
            }


            return BehaviorReturnCode.Success;
        }


    }
}
