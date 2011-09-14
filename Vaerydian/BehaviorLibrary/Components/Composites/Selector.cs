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
        /// -Returns Running if a behavior component returns Failure or Running
        /// -Returns Failure if all behavior components returned Failure or an error has occured
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
        public override BehaviorReturnCode Behave()
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
                        default:
                            selections++;
                            this.ReturnCode = BehaviorReturnCode.Failure;
                            return BehaviorReturnCode.Failure;
                    }
                }
                catch (Exception)
                {
                    selections++;
                    this.ReturnCode = BehaviorReturnCode.Failure;
                    return BehaviorReturnCode.Failure;
                }
            }

            selections = 0;
            this.ReturnCode = BehaviorReturnCode.Failure;
            return BehaviorReturnCode.Failure;
        }


    }
}
