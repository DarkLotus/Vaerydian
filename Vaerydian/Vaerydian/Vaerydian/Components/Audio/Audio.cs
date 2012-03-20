using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

namespace Vaerydian.Components.Audio
{
    class Audio : IComponent
    {

        private static int a_TypeID;
        private int a_EntityID;

        public Audio() { }

        public Audio(String soundEffectName, bool playNow, float volume)
        {
            a_SoundEffectName = soundEffectName;
            a_Play = playNow;
            a_Volume = volume;   
        }

        public int getEntityId()
        {
            return a_EntityID;
        }

        public int getTypeId()
        {
            return a_TypeID;
        }

        public void setEntityId(int entityId)
        {
            a_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            a_TypeID = typeId;
        }

        private String a_SoundEffectName;

        public String SoundEffectName
        {
            get { return a_SoundEffectName; }
            set { a_SoundEffectName = value; }
        }

        private bool a_Play = false;

        public bool Play
        {
            get { return a_Play; }
            set { a_Play = value; }
        }

        private bool a_Loop = false;

        public bool Loop
        {
            get { return a_Loop; }
            set { a_Loop = value; }
        }

        private float a_Volume = 1f;

        public float Volume
        {
            get { return a_Volume; }
            set { a_Volume = value; }
        }




    }
}
