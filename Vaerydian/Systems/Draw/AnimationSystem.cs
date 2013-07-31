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
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using ECSFramework;

using Vaerydian.Components.Utils;
using Vaerydian.Components.Graphical;
using Vaerydian.Components.Spatials;
using Vaerydian.Utils;

using Microsoft.Xna.Framework.Graphics;
using Vaerydian.Components.Characters;


namespace Vaerydian.Systems.Draw
{
    class AnimationSystem : EntityProcessingSystem
    {
        
        private ComponentMapper a_CharacterMapper;
        private ComponentMapper a_PositionMapper;
        private ComponentMapper a_ViewportMapper;
        private ComponentMapper a_LifeMapper;
        
        private GameContainer a_Container;
        private SpriteBatch a_SpriteBatch;

        private Dictionary<String, Texture2D> a_Textures = new Dictionary<string, Texture2D>();

        private Entity a_Camera;

        private Color a_Color;

        public AnimationSystem(GameContainer container)
        {
            a_Container = container;
        }
        
        public override void initialize()
        {
            a_CharacterMapper = new ComponentMapper(new Character(), e_ECSInstance);
            a_PositionMapper = new ComponentMapper(new Position(), e_ECSInstance);
            a_ViewportMapper = new ComponentMapper(new ViewPort(), e_ECSInstance);
            a_LifeMapper = new ComponentMapper(new Life(), e_ECSInstance);
        }

        protected override void preLoadContent(Bag<Entity> entities)
        {
            a_Camera = e_ECSInstance.TagManager.getEntityByTag("CAMERA");
            a_SpriteBatch = a_Container.SpriteBatch;
        }

        protected override void added(Entity entity)
        {
            Character character = (Character)a_CharacterMapper.get(entity);

               foreach (Bone bone in character.Skeletons[character.CurrentSkeleton].Bones)
            {
                if(!a_Textures.ContainsKey(bone.TextureName))
                    a_Textures.Add(bone.TextureName, a_Container.ContentManager.Load<Texture2D>(bone.TextureName));
            }
        }

        protected override void process(Entity entity)
        {
            Character character = (Character)a_CharacterMapper.get(entity);
            Position position = (Position)a_PositionMapper.get(entity);
            ViewPort viewPort = (ViewPort)a_ViewportMapper.get(a_Camera);
            Life life = (Life)a_LifeMapper.get(entity);

            a_SpriteBatch.Begin(SpriteSortMode.Deferred,BlendState.AlphaBlend,SamplerState.PointClamp,DepthStencilState.Default,RasterizerState.CullNone);

            float fade = 1;
			
			//FIX this to come from a component, not calc'd here
            a_Color = Color.White;

            if (life != null)
            {
                if (!life.IsAlive)
                {
                    fade = (1f - ((float)life.TimeSinceDeath / (float)life.DeathLongevity));
                    a_Color = Color.Red;
                }
            }

            foreach (Bone bone in character.Skeletons[character.CurrentSkeleton].Bones)
            {
                updateTime(bone, e_ECSInstance.ElapsedTime);
                a_SpriteBatch.Draw(a_Textures[bone.TextureName], position.Pos + getKeyPosition(bone, character.CurrentAnimtaion) - viewPort.getOrigin() + bone.RotationOrigin,
                    null, a_Color * fade, getKeyRotation(bone, character.CurrentAnimtaion), bone.RotationOrigin, 1f, SpriteEffects.None, 1f);
            }

            a_SpriteBatch.End();

        }

        protected override void cleanUp(Bag<Entity> entities)
        {
            
        }

        public void updateTime(Bone bone, int gameTime)
        {
            bone.ElapsedTime += gameTime;//.ElapsedGameTime.Milliseconds;

            if (bone.ElapsedTime >= bone.AnimationTime)
            {
                bone.ElapsedTime = 0;
            }
        }

        public Vector2 getKeyPosition(Bone bone, String animation)
        {
            for (int i = 0; i < bone.Animations[animation].Count; i++)
            {
                if (i > 0)
                {
                    if (bone.ElapsedTime <= bone.Animations[animation][i].KeyPercent * bone.AnimationTime && bone.ElapsedTime > bone.Animations[animation][i - 1].KeyPercent)
                        return bone.Origin + tweenKeyFramesPosition(bone, bone.Animations[animation][i - 1], bone.Animations[animation][i], bone.ElapsedTime);
                }
            }


            return bone.Origin + Vector2.Zero;
        }


        private Vector2 tweenKeyFramesPosition(Bone bone, KeyFrame a, KeyFrame b, int time)
        {
            float timeBetweenFrames = b.KeyPercent * bone.AnimationTime - a.KeyPercent * bone.AnimationTime;
            float timeAfterA = time - a.KeyPercent * bone.AnimationTime;
            float percentTween = timeAfterA / timeBetweenFrames;

            Vector2 aToB = b.KeyPosition - a.KeyPosition;

            return a.KeyPosition + (aToB * percentTween);
        }

        public float getKeyRotation(Bone bone, String animation)
        {
            for (int i = 0; i < bone.Animations[animation].Count; i++)
            {
                if (i > 0)
                {
                    if (bone.ElapsedTime <= bone.Animations[animation][i].KeyPercent * bone.AnimationTime && bone.ElapsedTime > bone.Animations[animation][i - 1].KeyPercent)
                        return tweenKeyFramesRotation(bone, bone.Animations[animation][i - 1], bone.Animations[animation][i], bone.ElapsedTime);
                }
            }
            return bone.Rotation;
        }


        private float tweenKeyFramesRotation(Bone bone, KeyFrame a, KeyFrame b, int time)
        {
            float timeBetweenFrames = b.KeyPercent * bone.AnimationTime - a.KeyPercent * bone.AnimationTime;
            float timeAfterA = time - a.KeyPercent * bone.AnimationTime;
            float percentTween = timeAfterA / timeBetweenFrames;

            float aToB = b.KeyRotation - a.KeyRotation;

            return a.KeyRotation + aToB * percentTween;
        }


    }
}
