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
using ECSFramework;

using Vaerydian.Components.Spatials;
using Vaerydian.Components.Utils;
using Vaerydian.Components.Characters;
using Vaerydian.Components.Graphical;

namespace Vaerydian.Systems.Update
{
	public class TargetingSystem : EntityProcessingSystem
	{
		ComponentMapper t_TargetMapper;
		ComponentMapper t_PositionMapper;
		ComponentMapper t_LifeMapper;
		ComponentMapper t_SpriteMapper;

		public TargetingSystem ()
		{
		}

		#region implemented abstract members of EntitySystem

		protected override void cleanUp (Bag<Entity> entities)
		{
		}

		public override void initialize ()
		{
			t_TargetMapper = new ComponentMapper (new Target(), e_ECSInstance);
			t_PositionMapper = new ComponentMapper (new Position(), e_ECSInstance);
			t_LifeMapper = new ComponentMapper (new Life (), e_ECSInstance);
			t_SpriteMapper = new ComponentMapper(new Sprite(), e_ECSInstance);

			base.initialize ();
		}

		protected override void preLoadContent (Bag<Entity> entities)
		{
		}

		#endregion

		#region implemented abstract members of EntityProcessingSystem

		protected override void process (Entity entity)
		{
			Target target = (Target)t_TargetMapper.get (entity);

			if (target == null)
				return;

			if (target.TargetEntity == null)
				return;

			Life entityLife = (Life)t_LifeMapper.get (target.TargetEntity);

			if (entityLife == null) {
				target.Active = false;
				return;
			}

			if (entityLife.IsAlive) {

				Position targetPos = (Position)t_PositionMapper.get (entity);
				Position entityPos = (Position)t_PositionMapper.get (target.TargetEntity);

				if (entityPos == null)
					return;

				targetPos.Pos = entityPos.Pos;
				targetPos.Offset = entityPos.Offset;
			} else {
				Sprite sprite = (Sprite)t_SpriteMapper.get (entity);
				sprite.Visible = false;
				target.Active = false;
				target.TargetEntity = null;
			}
		}

		#endregion
	}
}

