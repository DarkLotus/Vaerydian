using System;

using Microsoft.Xna.Framework;

using ECSFramework;

using Vaerydian.Components.Graphical;
using Vaerydian.Components.Spatials;
using Vaerydian.Components.Utils;
using Vaerydian.Utils;

namespace Vaerydian
{
	public class LightSystem : EntityProcessingSystem
	{
		private ComponentMapper l_LightMapper;
		private ComponentMapper l_PosMapper;
		private ComponentMapper l_GameMapMapper;

		private Entity l_GameMap;

		private GameMap l_Map;

		public LightSystem ()
		{
		}

		#region implemented abstract members of EntitySystem

		protected override void cleanUp (Bag<Entity> entities)
		{
			//throw new NotImplementedException ();
		}

		public override void initialize ()
		{
			base.initialize ();

			l_LightMapper = new ComponentMapper (new Light(), e_ECSInstance);
			l_PosMapper = new ComponentMapper (new Position(), e_ECSInstance);
			l_GameMapMapper = new ComponentMapper (new GameMap(), e_ECSInstance);
		}

		protected override void preLoadContent (Bag<Entity> entities)
		{
			l_GameMap = e_ECSInstance.TagManager.getEntityByTag ("MAP");
		}

		#endregion

		#region implemented abstract members of EntityProcessingSystem

		protected override void begin ()
		{
			l_Map = (GameMap) l_GameMapMapper.get (l_GameMap);


			base.begin ();
		}

		protected override void process (Entity entity)
		{
			Light light = (Light)l_LightMapper.get (entity);

			if (light == null)
				return;

			Position pos = (Position)l_PosMapper.get (entity);

			if (pos == null)
				return;

			int x, y;

			for (int i = - light.LightRadius; i < light.LightRadius; i++) {
				for(int j = - light.LightRadius; j < light.LightRadius; j++){

					//convert location to tilespace
					x = (((int)pos.Pos.X + (int) pos.Offset.X) / 32) + i;
					y = (((int)pos.Pos.Y + (int) pos.Offset.Y) / 32) + j;

					if(Vector2.Distance(pos.Pos - pos.Offset,new Vector2(x*32,y*32))>light.LightRadius*32)
						continue;

					Terrain terrain = l_Map.getTerrain(x,y);

					if(terrain == null)
						continue;


					//apply lighting to tile
					//float newLight = 1f/)*.05f);
                    float newLight = ((light.LightRadius * 32) - Vector2.Distance(pos.Pos,new Vector2(x*32,y*32))) / (light.LightRadius * 32);
					terrain.Lighting = newLight > terrain.Lighting ? newLight : terrain.Lighting;



					if(terrain.Lighting > 1f)
						terrain.Lighting = 1f;
				}
			}

		}

		#endregion
	}
}

