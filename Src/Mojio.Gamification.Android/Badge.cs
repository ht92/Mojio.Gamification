using System;
using System.Collections.Generic;
using Newtonsoft.Json;

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

		[JsonProperty]
		protected string mBadgeName;
		[JsonProperty]
		protected string mBadgeDescription;
		[JsonProperty]
		protected int mBadgeDrawableRes;
		[JsonProperty]
		protected DateTime? mBadgeUnlockDate;
		[JsonProperty]
		protected Dictionary<string, double> mProperties;
	
		[JsonConstructor]
		public Badge ()
		{
		}

		public Badge (string badgeName, string badgeDescription, int badgeDrawable)
		{
			mBadgeName = badgeName;
			mBadgeDescription = badgeDescription;
			mBadgeDrawableRes = badgeDrawable;
			mProperties = new Dictionary<string, double> ();
		}

		public virtual string GetName ()
		{
			return mBadgeName;
		}

		public virtual string GetDisplayName ()
		{
			return mBadgeName;
		}

		public virtual string GetDescription ()
		{
			return mBadgeDescription;
		}

		public int GetDrawableResource ()
		{
			return mBadgeDrawableRes;
		}

		public DateTime? GetUnlockDate ()
		{
			return mBadgeUnlockDate;
		}

		public virtual bool IsUnlocked ()
		{
			return mBadgeUnlockDate.HasValue;
		}

		public virtual bool IsFullyUnlocked ()
		{
			return IsUnlocked ();
		}

		public void SetProperty (string name, double value)
		{
			if (mProperties.ContainsKey (name)) {
				mProperties [name] = value;
			} else {
				mProperties.Add (name, value);
			}
		}

		public void IncrementProperty (string name, double value)
		{
			if (mProperties.ContainsKey (name)) {
				mProperties [name] += value;
			} else {
				SetProperty (name, value);
			}
		}

		public double? GetProperty (string name)
		{
			double? value = null;
			if (mProperties.ContainsKey (name)) {
				value = mProperties [name];
			}
			return value;
		}

		public virtual void UpdateAchievement ()
		{
			if (!IsFullyUnlocked ()) {
				getUpdateAchievementDelegate ().Invoke ();
			}
		}

		public virtual void CheckAchievement () 
		{
			if (IsFullyUnlocked ()) {
				return;
			}
			if (getCheckAchievementDelegate ().Invoke ()) {
				mBadgeUnlockDate = DateTime.Now;
				GamificationApp.GetInstance ().MyNotificationService.IssueBadgeNotification (this);
			};
		}
			
		protected AchievementManager.CheckAchievementDelegate getCheckAchievementDelegate ()
		{
			return AchievementManager.badgeCheckDelegateMapping [this.GetName ()];
		}

		protected AchievementManager.UpdateAchievementDelegate getUpdateAchievementDelegate ()
		{
			return AchievementManager.badgeUpdateDelegateMapping [this.GetName ()];
		}
	}
}