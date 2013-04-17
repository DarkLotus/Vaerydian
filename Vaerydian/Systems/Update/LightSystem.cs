using System;

using ECSFramework;

namespace Vaerydian
{
	public class LightSystem : EntityProcessingSystem
	{
		public LightSystem ()
		{
		}

		#region implemented abstract members of EntitySystem

		protected override void cleanUp (Bag<Entity> entities)
		{
			throw new NotImplementedException ();
		}

		protected override void preLoadContent (Bag<Entity> entities)
		{
			throw new NotImplementedException ();
		}

		#endregion

		#region implemented abstract members of EntityProcessingSystem

		protected override void process (Entity entity)
		{
			throw new NotImplementedException ();
		}

		#endregion
	}
}

