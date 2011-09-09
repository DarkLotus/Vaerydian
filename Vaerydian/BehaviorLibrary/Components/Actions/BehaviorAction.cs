using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorLibrary.Components.Actions
{
    public class BehaviorAction : BehaviorComponent
    {

        private Func<BehaviorReturnCode> ba_Action;


        public BehaviorAction(Func<BehaviorReturnCode> action)
        {
            ba_Action = action;
        }

        public override BehaviorReturnCode Behave()
        {
            try
            {
                switch (ba_Action.Invoke())
                {
                    case BehaviorReturnCode.Success:
                        this.ReturnCode = BehaviorReturnCode.Success;
                        return this.ReturnCode;
                    case BehaviorReturnCode.Failure:
                        this.ReturnCode = BehaviorReturnCode.Failure;
                        return this.ReturnCode;
                    case BehaviorReturnCode.Running:
                        this.ReturnCode = BehaviorReturnCode.Running;
                        return this.ReturnCode;
                    default:
                        this.ReturnCode = BehaviorReturnCode.Failure;
                        return this.ReturnCode;
                }
            }
            catch (Exception)
            {
                this.ReturnCode = BehaviorReturnCode.Failure;
                return this.ReturnCode;
            }
        }

    }
}
