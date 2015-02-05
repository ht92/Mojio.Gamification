using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	public class LevelBadge : Badge
	{
		public static int DEFAULT_MAX_LEVEL = 3;

		private int mLevel;
		private int mMaxLevel = DEFAULT_MAX_LEVEL;

		[JsonConstructor]
		public LevelBadge () 
			: base ()
		{
		}

		public LevelBadge (string badgeName, string badgeDescription, int badgeDrawable)
			: base (badgeName, badgeDescription, badgeDrawable)
		{
		}

		public LevelBadge (string badgeName, string badgeDescription, int badgeDrawable, int maxLevel)
			: base (badgeName, badgeDescription, badgeDrawable)
		{
			mMaxLevel = maxLevel;
		}

		public override string GetDisplayName ()
		{
			return String.Format ("{0} {1}", base.GetDisplayName (), GetLevel ());
		}

		public int GetLevel ()
		{
			return mLevel;
		}

		public int GetMaxLevel ()
		{
			return mMaxLevel;
		}

		public void LevelUp ()
		{
			mLevel++;
			mBadgeUnlockDate = DateTime.Now;
		}

		public override bool IsUnlocked ()
		{
			return base.IsUnlocked () && mLevel > 0;
		}

		public override bool IsFullyUnlocked ()
		{
			return mLevel == mMaxLevel;
		}

		public override void CheckAchievement ()
		{
			if (IsFullyUnlocked ()) {
				return;
			}
			if (getCheckAchievementDelegate ().Invoke ()) {
				LevelUp ();
			}
		}
	}
}

