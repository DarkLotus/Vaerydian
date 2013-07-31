/*
 Author:
      Thomas H. Jonell <@Net_Gnome>
 
 Copyright (c) 2013 Thomas H. Jonell

 This program is free software: you can redistribute it and/or modify
 it under the terms of the GNU Lesser General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.

 This program is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU Lesser General Public License for more details.

 You should have received a copy of the GNU Lesser General Public License
 along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;

using Microsoft.Xna.Framework;

using ECSFramework;

using Vaerydian.Components.Graphical;
using Vaerydian.Components.Spatials;
using Vaerydian.Components.Utils;
using Vaerydian.Utils;
using System.Collections.Generic;

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

			int x, y, px, py;

			for (int i = - light.LightRadius; i < light.LightRadius; i++) {
				for(int j = - light.LightRadius; j < light.LightRadius; j++){

					//convert location to tilespace
					px = ((int)pos.Pos.X + (int) pos.Offset.X) / 32;
					py = ((int)pos.Pos.Y + (int) pos.Offset.Y) / 32;
					x = px + i;
					y = py + j;

					if(Vector2.Distance(pos.Pos - pos.Offset,new Vector2(x*32,y*32))>light.LightRadius*32)
						continue;

					if(!isNotObscured(l_Map, x, y, px,py))
						continue;

					Terrain terrain = l_Map.getTerrain(x,y);

					if(terrain == null)
						continue;


					//apply lighting to tile
                    float newLight = ((light.LightRadius * 32) - Vector2.Distance(pos.Pos,new Vector2(x*32,y*32))) / (light.LightRadius * 32);
					terrain.Lighting = newLight > terrain.Lighting ? newLight : terrain.Lighting;

					if(terrain.Lighting > 1f)
						terrain.Lighting = 1f;
				}
			}

		}

		#endregion

		private List<Vector2> bress(int x0, int y0, int x1, int y1){
			int dx = x1 - x0;
			int dy = y1 - y0;
			float err = 0f;
			float derr = Math.Abs ((float)dy / (float)dx);

			List<Vector2> line = new List<Vector2> ();
			//line.Add (new Vector2 (x0, y0));

			int sx, sy;
			
			if (x0 < x1)
				sx = 1;
			else
				sx = -1;
			
			if (y0 < y1)
				sy = 1;
			else
				sy = -1;

			int y = y0;

			if (sx == 1) {
				for (int x = x0; x <= x1; x++) {
					line.Add (new Vector2 (x, y));

					err = err + derr;

					if (err >= 0.5f) {
						y = y + sy;
						err = err - 1.0f;
					}
				}
			} else {
				for (int x = x0; x >= x1; x--) {
					line.Add (new Vector2 (x, y));
					
					err = err + derr;
					
					if (err >= 0.5f) {
						y = y + sy;
					 	err = err - 1.0f;
					}
				}
			}

			return line;
		}

		private List<Vector2> bressenham(int x0, int y0, int x1, int y1){
			int dx = Math.Abs (x1 - x0);
			int dy = Math.Abs (y1 - y0);
			int sx, sy;

			if (x0 < x1)
				sx = 1;
			else
				sx = -1;

			if (y0 < y1)
				sy = 1;
			else
				sy = -1;

			int err = dx - dy;
			int e2;

			List<Vector2> line = new List<Vector2> ();

			while (true) {
				line.Add(new Vector2(x0,y0));

				if(x0 == x1 && y0 == y1)
					return line;

				e2 = 2*err;

				if(e2 >-dy){
					err = err - dy;
					x0 = x0 + sx;
				}

				if(x0 == x1 && y0 == y1){
					line.Add(new Vector2(x0,y0));
					return line;
				}

				if(e2 < dx){
					err = err + dx;
					y0 = y0 + sy;
				}
			}
		}

		private bool isNotObscured(GameMap map, int x, int y, int px, int py){
			List<Vector2> line = bressenham (x, y, px, py);

			if (line.Count < 2) {
				Terrain terrain = map.getTerrain ((int)line [0].X, (int)line [0].Y);
				if (terrain == null)
					return false;
				if (terrain.IsBlocking)
					return false;
			} else {

				for (int i = 1; i < line.Count; i++) {
					Terrain terrain = map.getTerrain ((int)line [i].X, (int)line [i].Y);
					if (terrain == null)
						return false;
					if (terrain.IsBlocking)
						return false;
				}
			}

			return true;
		}
	}
}

