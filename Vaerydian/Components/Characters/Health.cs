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

namespace Vaerydian.Components.Characters
{
    class Health: IComponent
    {
        private static int h_TypeID;
        private int h_EntityID;

        public Health() { }

        public Health(int max)
        {
            h_MaxHealth = max;
            h_CurrentHealth = max;
        }

        public int getEntityId()
        {
            return h_EntityID;
        }

        public int getTypeId()
        {
            return h_TypeID;
        }

        public void setEntityId(int entityId)
        {
            h_EntityID = entityId;
        }

        public void setTypeId(int typeId)
        {
            h_TypeID = typeId;
        }

        private int h_MaxHealth;

        public int MaxHealth
        {
            get { return h_MaxHealth; }
            set { h_MaxHealth = value; }
        }

        private int h_CurrentHealth;

        public int CurrentHealth
        {
            get { return h_CurrentHealth; }
            set { h_CurrentHealth = value; }
        }

        private int h_RecoveryRate;

        public int RecoveryRate
        {
            get { return h_RecoveryRate; }
            set { h_RecoveryRate = value; }
        }

        private int h_RecoveryAmmount;

        public int RecoveryAmmount
        {
            get { return h_RecoveryAmmount; }
            set { h_RecoveryAmmount = value; }
        }

        private int h_TimeSinceLastRecover;

        public int TimeSinceLastRecover
        {
            get { return h_TimeSinceLastRecover; }
            set { h_TimeSinceLastRecover = value; }
        }
    }
}
