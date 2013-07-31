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

//using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Audio;

using ECSFramework;



using Vaerydian;
using Vaerydian.Components.Audio;


namespace Vaerydian.Systems.Update
{
    class AudioSystem : EntityProcessingSystem
    {
        private GameContainer a_Container;

        private ComponentMapper a_AudioMapper;

        private Dictionary<String, SoundEffect> a_SoundEffects = new Dictionary<String, SoundEffect>();

        public AudioSystem(GameContainer container) : base() 
        {
            a_Container = container;
        }

        public override void initialize()
        {
            a_AudioMapper = new ComponentMapper(new Audio(), e_ECSInstance);
        }

        protected override void preLoadContent(Bag<Entity> entities)
        {
            for (int i = 0; i < entities.Size(); i++)
            {
                Audio audio = (Audio) a_AudioMapper.get(entities.Get(i));
                if(!a_SoundEffects.ContainsKey(audio.SoundEffectName))
                    a_SoundEffects.Add(audio.SoundEffectName, a_Container.ContentManager.Load<SoundEffect>(audio.SoundEffectName));
            }
        }

        protected override void cleanUp(Bag<Entity> entities) { }

        protected override void added(Entity entity)
        {
            Audio audio = (Audio)a_AudioMapper.get(entity);
            if (!a_SoundEffects.ContainsKey(audio.SoundEffectName))
                a_SoundEffects.Add(audio.SoundEffectName, a_Container.ContentManager.Load<SoundEffect>(audio.SoundEffectName));
        }

        protected override void process(Entity entity)
        {
            Audio audio = (Audio)a_AudioMapper.get(entity);

            if (!audio.Play)
                return;

            a_SoundEffects[audio.SoundEffectName].Play(audio.Volume, audio.Pitch, 0f);

            audio.Play = false;
            e_ECSInstance.deleteEntity(entity);
        }


    }
}
