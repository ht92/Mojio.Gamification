using System;
using System.Collections.Generic;

using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	public class Badge
	{
		public enum BadgeType
		{
			NORMAL = 0,
			COUNT = 1,
			LEVEL = 2
		}
				
		public delegate bool CheckAchievementDelegate ();
		protected UserBadge mBadgeModel;
		private string mBadgeDescription;
		private int mBadgeDrawableRes;
		private CheckAchievementDelegate mCheckAchievementDelegate;

		public Badge (UserBadge badgeModel, int badgeDrawable, string badgeDescription, CheckAchievementDelegate checkMethod)
		{
			mBadgeModel = badgeModel;
			mBadgeDrawableRes = badgeDrawable;
			mBadgeDescription = badgeDescription;
			mCheckAchievementDelegate = checkMethod;
		}

		public UserBadge GetBadgeModel()
		{
			return mBadgeModel;
		}

		public String GetName ()
		{
			return mBadgeModel.badgeName;
		}

		public String GetDescription ()
		{
			return mBadgeDescription;
		}

		public int GetDrawableResource ()
		{
			return mBadgeDrawableRes;
		}

		public bool IsUnlocked ()
		{
			return mBadgeModel.badgeLevel > 0;
		}

		public void CheckAchievement () 
		{
			if (IsUnlocked ()) {
				return;
			}
			if (mCheckAchievementDelegate ()) {
				mBadgeModel.badgeLevel++;
			};
		}
	}
}

