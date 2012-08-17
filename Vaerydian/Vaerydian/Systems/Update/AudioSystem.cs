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
