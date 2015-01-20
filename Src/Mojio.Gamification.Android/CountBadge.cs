using System;
using System.Collections.Generic;
using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	public class CountBadge : Badge
	{
		public CountBadge (UserBadge badgeModel, int badgeDrawable, string badgeDescription, CheckAchievementDelegate checkMethod) :
		base (badgeModel, badgeDrawable, badgeDescription, checkMethod)
		{
		}

		public int GetCount ()
		{
			return this.mBadgeModel.badgeLevel;
		}
	}
}

