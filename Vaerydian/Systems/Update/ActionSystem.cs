using System;

using ECSFramework;

using Vaerydian.Components.Actions;
using Vaerydian.Utils;

namespace Vaerydian
{
	public class ActionSystem : EntityProcessingSystem
	{
		private ComponentMapper a_ActionMapper;

		public ActionSystem ()
		{
		}

		public override void initialize ()
		{
			a_ActionMapper = new ComponentMapper (new VAction (), e_ECSInstance);
		}

		protected override void preLoadContent (Bag<Entity> entities)
		{
			throw new System.NotImplementedException ();
		}

		protected override void process (Entity entity)
		{
			VAction action = (VAction) a_ActionMapper.get(entity);

			action.doAction();

			e_ECSInstance.deleteEntity (entity);
		}

		protected override void removed (Entity entity)
		{
			base.removed (entity);
		}

		protected override void cleanUp (Bag<Entity> entities)
		{
			//do nothing for now
		}

		private void foo(){}
	}
}

