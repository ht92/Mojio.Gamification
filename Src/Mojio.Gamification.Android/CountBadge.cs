using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	public class CountBadge : Badge
	{
		private int mCount;

		[JsonConstructor]
		public CountBadge () 
			: base ()
		{
		}

		public CountBadge (string badgeName, string badgeDescription, int badgeDrawable)
			: base (badgeName, badgeDescription, badgeDrawable)
		{
		}

		public int GetCount ()
		{
			return mCount;
		}

		public override bool IsFullyUnlocked ()
		{
			return false;
		}

		public override void CheckAchievement ()
		{
			if (!getCheckAchievementDelegate ().Invoke ()) return;
			if (mCount == 0) mBadgeUnlockDate = DateTime.Now;
			mCount++;
		}
	}
}

