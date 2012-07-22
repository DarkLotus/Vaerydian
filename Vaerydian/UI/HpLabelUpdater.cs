using System;

using Glimpse.Controls;

using ECSFramework;

using Vaerydian.Components.Characters;
using Vaerydian.Components.Spatials;

namespace Vaerydian
{
	public class HpLabelUpdater
	{
		private ECSInstance h_ECSInstance;
		private ComponentMapper h_HealthMapper;
		//private ComponentMapper h_PositionMapper;

		public HpLabelUpdater (ECSInstance ecsInstnace)
		{
			h_ECSInstance = ecsInstnace;
			h_HealthMapper = new ComponentMapper(new Health(), h_ECSInstance);
		}

		public void updateHandler (IControl control, InterfaceArgs args)
		{

			Health health = (Health) h_HealthMapper.get(control.Caller);

			GLabel label = (GLabel) control;
			label.Text = health.CurrentHealth + " / " + health.MaxHealth;
			label.resize();
		}
	}
}

