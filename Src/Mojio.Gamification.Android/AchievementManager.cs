using System;
using System.Collections.Generic;
using Mojio.Gamification.Core;
using Mojio.Gamification.Android;

namespace Mojio.Gamification.Android
{
	public class AchievementManager
	{
		private List<Badge> mBadgeCollection = new List<Badge> ();
		private UserBadgeRepository _userBadgeRepository;
		private StatisticsManager _statsManager;

		private static AchievementManager _instance;

		public static AchievementManager GetInstance ()
		{
			if (_instance == null) {
				_instance = new AchievementManager ();
			}
			return _instance;
		}

		private AchievementManager ()
		{
			_userBadgeRepository = GamificationApp.GetInstance ().MyUserBadgeRepository;
			_statsManager = GamificationApp.GetInstance ().MyStatisticsManager;
			attachListeners ();
			SyncFromDb ();
		}

		public void SyncFromDb ()
		{
			mBadgeCollection.Clear ();
			List<UserBadge> collection = _userBadgeRepository.GetUserBadges ();
			foreach (UserBadge badgeModel in collection) {
				mBadgeCollection.Add (BadgeFactory.CreateBadge (badgeModel));
			}
		}

		public void SyncToDb ()
		{
			List<UserBadge> collection = new List<UserBadge> ();
			foreach (Badge badge in mBadgeCollection) {
				collection.Add (badge.GetBadgeModel ());
			}
			_userBadgeRepository.UpdateBadges (collection);
		}

		public List<Badge> GetBadgeCollection ()
		{
			return mBadgeCollection;
		}

		public List<Badge> GetUnlockedBadgeCollection ()
		{
			List<Badge> unlockedBadges = new List<Badge> ();
			foreach (Badge badge in mBadgeCollection) {
				if (badge.IsUnlocked ()) {
					unlockedBadges.Add (badge);
				}
			}
			return unlockedBadges;
		}

		public List<Badge> GetLockedBadgeCollection ()
		{
			List<Badge> lockedBadges = new List<Badge> ();
			foreach (Badge badge in mBadgeCollection) {
				if (!badge.IsUnlocked ()) {
					lockedBadges.Add (badge);
				}
			}
			return lockedBadges;
		}

		private void attachListeners ()
		{
			_statsManager.StatisticsUpdatedEvent += (object sender, EventArgs e) => {
				CheckAchievements ();
			};
		}

		public void CheckAchievements ()
		{
			List<Badge> lockedBadges = GetLockedBadgeCollection ();
			foreach (Badge lockedBadge in lockedBadges) {
				lockedBadge.CheckAchievement ();
			}
			SyncToDb ();
		}
			
		public static bool CheckNullAchievement ()
		{
			return true;
		}

		public static bool CheckFirstTripAchievement () 
		{
			return GamificationApp.GetInstance ().MyStatisticsManager.MyStats.totalTrips >= 1;
		}

		public static bool CheckPerfectTripAchievement ()
		{
			double lastTripScore = 0;
			return lastTripScore == 100;
		}

		public static bool CheckSafetyFirstAchievement ()
		{
			double totalDistance = GamificationApp.GetInstance ().MyStatisticsManager.MyStats.totalDistance;
			double safetyScore = GamificationApp.GetInstance ().MyStatisticsManager.MyStats.safetyScore;
			return totalDistance >= 1000 && safetyScore >= 90;
		}

		public static bool CheckEfficientAchievement ()
		{
			double totalDistance = GamificationApp.GetInstance ().MyStatisticsManager.MyStats.totalDistance;
			double efficiencyScore = GamificationApp.GetInstance ().MyStatisticsManager.MyStats.efficiencyScore;
			return totalDistance >= 1000 && efficiencyScore >= 90;
		}
	
		public static bool CheckHighAchieverAchievement () 
		{
			double totalDistance = GamificationApp.GetInstance ().MyStatisticsManager.MyStats.totalDistance;
			double overallScore = GamificationApp.GetInstance ().MyStatisticsManager.OverallScore;
			return totalDistance >= 1000 && overallScore >= 90;
		}

		public static bool CheckSelfImprovementAchievement () 
		{
			int improvementStreak = 0;
			return improvementStreak >= 5;
		}

		public static bool CheckPerfectionistAchievement () 
		{
			int perfectStreak = 0;
			return perfectStreak >= 5;
		}
	}
}

