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

        public Audio(String soundEffectName, bool playNow, float volume, float pitch)
        {
            a_SoundEffectName = soundEffectName;
            a_Play = playNow;
            a_Volume = volume;
            a_Pitch = pitch;
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

        private float a_Pitch = 0f;

        public float Pitch
        {
            get { return a_Pitch; }
            set { a_Pitch = value; }
        }


    }
}
