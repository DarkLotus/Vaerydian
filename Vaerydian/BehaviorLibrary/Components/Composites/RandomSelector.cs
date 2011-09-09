using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorLibrary.Components.Composites
{
    public class RandomSelector : BehaviorComponent
    {

        private BehaviorComponent[] r_Behaviors;

        private Random r_Random = new Random();

        /// <summary>
        /// Randomly selects and performs one of the passed behaviors
        /// -Returns Success if selected behavior returns Success
        /// -Returns Failure if selected behavior returns Failure
        /// -Returns Running if selected behavior returns Running
        /// </summary>
        /// <param name="behaviors">one to many behavior components</param>
        public RandomSelector(params BehaviorComponent[] behaviors) 
        {
            r_Behaviors = behaviors;
        }

        /// <summary>
        /// performs the given behavior
        /// </summary>
        /// <returns>the behaviors return code</returns>
        public override BehaviorReturnCode Behave()
        {
            try
            {
                switch (r_Behaviors[r_Random.Next(0, r_Behaviors.Length - 1)].Behave())
                {
                    case BehaviorReturnCode.Failure:
                        this.ReturnCode = BehaviorReturnCode.Failure;
                        return BehaviorReturnCode.Failure;
                    case BehaviorReturnCode.Success:
                        this.ReturnCode = BehaviorReturnCode.Success;
                        return BehaviorReturnCode.Success;
                    case BehaviorReturnCode.Running:
                        this.ReturnCode = BehaviorReturnCode.Running;
                        return BehaviorReturnCode.Running;
                }
            }
            catch (Exception)
            {
                this.ReturnCode = BehaviorReturnCode.Failure;
                return BehaviorReturnCode.Failure;
            }

            this.ReturnCode = BehaviorReturnCode.Failure;
            return BehaviorReturnCode.Failure;
        }

    }
}
