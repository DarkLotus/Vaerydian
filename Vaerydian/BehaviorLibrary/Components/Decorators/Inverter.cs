using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorLibrary.Components.Decorators
{
    public class Inverter : BehaviorComponent
    {

        private BehaviorComponent d_Behavior;

        /// <summary>
        /// inverts the given behavior
        /// -Returns Success on Failure or Error
        /// -Returns Failure on Success 
        /// -Returns Running on Running
        /// </summary>
        /// <param name="behavior"></param>
        public Inverter(BehaviorComponent behavior) 
        {
            d_Behavior = behavior;
        }

        /// <summary>
        /// performs the given behavior
        /// </summary>
        /// <returns>the behaviors return code</returns>
        public override BehaviorReturnCode Behave()
        {
            try
            {
                switch (d_Behavior.Behave())
                {
                    case BehaviorReturnCode.Failure:
                        this.ReturnCode = BehaviorReturnCode.Success;
                        return BehaviorReturnCode.Success;
                    case BehaviorReturnCode.Success:
                        this.ReturnCode = BehaviorReturnCode.Failure;
                        return BehaviorReturnCode.Failure;
                    case BehaviorReturnCode.Running:
                        this.ReturnCode = BehaviorReturnCode.Running;
                        return BehaviorReturnCode.Running;
                }
            }
            catch (Exception)
            {
                this.ReturnCode = BehaviorReturnCode.Success;
                return BehaviorReturnCode.Success;
            }

            this.ReturnCode = BehaviorReturnCode.Success;
            return BehaviorReturnCode.Success;

        }

    }
}
