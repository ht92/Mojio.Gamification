using System;
using System.Collections.Generic;

using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	public class BadgeFactory
	{
		public static Badge CreateBadge (UserBadge badgeModel)
		{
			int badgeId = badgeModel.badgeId;
			switch (badgeId) {
			case 0:	return CreateBadge (badgeModel, Resource.Drawable.Icon, "Complete first trip.", AchievementManager.CheckFirstTripAchievement);
			case 1:	return CreateBadge (badgeModel, Resource.Drawable.Icon, "Complete a trip with a perfect score.", AchievementManager.CheckPerfectTripAchievement);
			case 2: return CreateBadge (badgeModel, Resource.Drawable.Icon, "Drive over 1000km and maintain a safety score over 90.", AchievementManager.CheckSafetyFirstAchievement);
			case 3: return CreateBadge (badgeModel, Resource.Drawable.Icon, "Drive over 1000km and maintain an efficiency score over 90.", AchievementManager.CheckEfficientAchievement);
			case 4: return CreateBadge (badgeModel, Resource.Drawable.Icon, "Drive over 1000km and maintain an overall score over 90.", AchievementManager.CheckHighAchieverAchievement);
			case 5: return CreateBadge (badgeModel, Resource.Drawable.Icon, "Drive over 10000km and maintain an overall score over 50.", AchievementManager.CheckVeteranAchievement);
			case 6: return CreateBadge (badgeModel, Resource.Drawable.Icon, "5 straight trips with improving scores.", AchievementManager.CheckSelfImprovementAchievement);
			case 7: return CreateBadge (badgeModel, Resource.Drawable.Icon, "5 straight trips with perfect scores.", AchievementManager.CheckPerfectionistAchievement);
			case 8: return CreateBadge (badgeModel, Resource.Drawable.Icon, "5 straight trips without an accident.", AchievementManager.CheckUntouchableAchievement);
			case 9: return CreateBadge (badgeModel, Resource.Drawable.Icon, "Test badge number 1.", AchievementManager.CheckNullAchievement);
			case 10: return CreateBadge (badgeModel, Resource.Drawable.Icon, "Test badge number 2.", AchievementManager.CheckNullAchievement);
			case 11: return CreateBadge (badgeModel, Resource.Drawable.Icon, "Test badge number 3.", AchievementManager.CheckNullAchievement);
			default: throw new ArgumentException (String.Format ("Unable to create badge - {0}.", badgeId));
			}
		}

		private static Badge CreateBadge (UserBadge badgeModel, int badgeDrawable, string badgeDescription, Badge.CheckAchievementDelegate checkMethod)
		{
			Badge.BadgeType type = (Badge.BadgeType) badgeModel.badgeType;
			switch (type) {
			case Badge.BadgeType.NORMAL: 	return new Badge (badgeModel, badgeDrawable, badgeDescription, checkMethod);
			case Badge.BadgeType.COUNT: 	return new CountBadge (badgeModel, badgeDrawable, badgeDescription, checkMethod);
			case Badge.BadgeType.LEVEL:		return new Badge (badgeModel, badgeDrawable, badgeDescription, checkMethod);
			default: 						throw new ArgumentException (String.Format ("Invalid badge type - {0}.", type.ToString ()));
			}
		}
	}
}

