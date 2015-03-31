using System;
using System.Collections.Generic;
using Mojio.Gamification.Core;
using Mojio.Gamification.Android;

namespace Mojio.Gamification.Android
{
	public class AchievementManager
	{
		public delegate void UpdateAchievementDelegate ();
		public delegate bool CheckAchievementDelegate ();

		//Badge Names
		public const string BADGE_FIRST_TRIP_NAME = "First Trip";
		public const string BADGE_PERFECT_TRIP_NAME = "Perfect Trip";
		public const string BADGE_SAFETY_FIRST_NAME = "Safety First";
		public const string BADGE_EFFICIENT_NAME = "Efficient";
		public const string BADGE_HIGH_ACHIEVER_NAME = "High Achiever";
		public const string BADGE_VETERAN_NAME = "Veteran";
		public const string BADGE_SELF_IMPROVEMENT_NAME = "Self-Improvement";
		public const string BADGE_PERFECTIONIST_NAME = "Perfectionist";
		public const string BADGE_UNTOUCHABLE_NAME = "Untouchable";

		//Badge Descriptions
		private const string BADGE_FIRST_TRIP_DESCRIPTION = "Complete first trip.";
		private const string BADGE_PERFECT_TRIP_DESCRIPTION = "Complete a trip with a perfect score.";
		private const string BADGE_SAFETY_FIRST_DESCRIPTION = "Drive at least 1000km and maintain a safety score of at least 90.";
		private const string BADGE_EFFICIENT_DESCRIPTION = "Drive at least 1000km and maintain an efficiency score of at least 90.";
		private const string BADGE_HIGH_ACHIEVER_DESCRIPTION = "Drive at least 1000km and maintain an overall score of at least 90.";
		private const string BADGE_VETERAN_DESCRIPTION = "Drive at least 10000km and maintain an overall score of at least 50.";
		private const string BADGE_SELF_IMPROVEMENT_DESCRIPTION = "5 trips in a row with improving scores.";
		private const string BADGE_PERFECTIONIST_DESCRIPTION = "5 trips in a row with perfect scores.";
		private const string BADGE_UNTOUCHABLE_DESCRIPTION = "5 trips in a row without an accident.";

		//Badges
		private static Badge BADGE_FIRST_TRIP = createBadge (BADGE_FIRST_TRIP_NAME, BADGE_FIRST_TRIP_DESCRIPTION, Resource.Drawable.badge_firstTrip);
		private static Badge BADGE_PERFECT_TRIP = createBadge (BADGE_PERFECT_TRIP_NAME, BADGE_PERFECT_TRIP_DESCRIPTION, Resource.Drawable.badge_perfectTrip);
		private static Badge BADGE_SAFETY_FIRST = createBadge (BADGE_SAFETY_FIRST_NAME, BADGE_SAFETY_FIRST_DESCRIPTION, Resource.Drawable.badge_safetyFirst);
		private static Badge BADGE_EFFICIENT = createBadge (BADGE_EFFICIENT_NAME, BADGE_EFFICIENT_DESCRIPTION, Resource.Drawable.badge_efficiency);
		private static Badge BADGE_HIGH_ACHIEVER = createBadge (BADGE_HIGH_ACHIEVER_NAME, BADGE_HIGH_ACHIEVER_DESCRIPTION, Resource.Drawable.badge_highAchiever);
		private static Badge BADGE_VETERAN = createBadge (BADGE_VETERAN_NAME, BADGE_VETERAN_DESCRIPTION, Resource.Drawable.badge_veteran1);
		private static Badge BADGE_SELF_IMPROVEMENT = createBadge (BADGE_SELF_IMPROVEMENT_NAME, BADGE_SELF_IMPROVEMENT_DESCRIPTION, Resource.Drawable.badge_selfImprovement1);
		private static Badge BADGE_PERFECTIONIST = createBadge (BADGE_PERFECTIONIST_NAME, BADGE_PERFECTIONIST_DESCRIPTION, Resource.Drawable.badge_perfectionist1);
		private static Badge BADGE_UNTOUCHABLE = createBadge (BADGE_UNTOUCHABLE_NAME, BADGE_UNTOUCHABLE_DESCRIPTION, Resource.Drawable.logo);

		//Badge Unlock Check Delegates
		private static CheckAchievementDelegate BADGE_FIRST_TRIP_CHECK_DELEGATE = CheckFirstTripAchievement;
		private static CheckAchievementDelegate BADGE_PERFECT_TRIP_CHECK_DELEGATE = CheckPerfectTripAchievement;
		private static CheckAchievementDelegate BADGE_SAFETY_FIRST_CHECK_DELEGATE = CheckSafetyFirstAchievement;
		private static CheckAchievementDelegate BADGE_EFFICIENT_CHECK_DELEGATE = CheckEfficientAchievement;
		private static CheckAchievementDelegate BADGE_HIGH_ACHIEVER_CHECK_DELEGATE = CheckHighAchieverAchievement;
		private static CheckAchievementDelegate BADGE_VETERAN_CHECK_DELEGATE = CheckVeteranAchievement;
		private static CheckAchievementDelegate BADGE_SELF_IMPROVEMENT_CHECK_DELEGATE = CheckSelfImprovementAchievement;
		private static CheckAchievementDelegate BADGE_PERFECTIONIST_CHECK_DELEGATE = CheckPerfectionistAchievement;
		private static CheckAchievementDelegate BADGE_UNTOUCHABLE_CHECK_DELEGATE = CheckUntouchableAchievement;

		//Badge Unlock Update Delegates
		private static UpdateAchievementDelegate BADGE_FIRST_TRIP_UPDATE_DELEGATE = UpdateNullAchievement;
		private static UpdateAchievementDelegate BADGE_PERFECT_TRIP_UPDATE_DELEGATE = UpdateNullAchievement;
		private static UpdateAchievementDelegate BADGE_SAFETY_FIRST_UPDATE_DELEGATE = UpdateNullAchievement;
		private static UpdateAchievementDelegate BADGE_EFFICIENT_UPDATE_DELEGATE = UpdateNullAchievement;
		private static UpdateAchievementDelegate BADGE_HIGH_ACHIEVER_UPDATE_DELEGATE = UpdateNullAchievement;
		private static UpdateAchievementDelegate BADGE_VETERAN_UPDATE_DELEGATE = UpdateNullAchievement;
		private static UpdateAchievementDelegate BADGE_SELF_IMPROVEMENT_UPDATE_DELEGATE = UpdateNullAchievement;
		private static UpdateAchievementDelegate BADGE_PERFECTIONIST_UPDATE_DELEGATE = UpdateNullAchievement;
		private static UpdateAchievementDelegate BADGE_UNTOUCHABLE_UPDATE_DELEGATE = UpdateUntouchableAchievement;

		private const int BADGE_SELF_IMPROVEMENT_STREAK = 5;
		private const int BADGE_PERFECTIONIST_STREAK = 5;
		private const int BADGE_UNTOUCHABLE_STREAK = 10000;
		private const string BADGE_UNTOUCHABLE_KM_PROP = "distanceTravelled";

		public static Dictionary<string, CheckAchievementDelegate> badgeCheckDelegateMapping = new Dictionary<string, CheckAchievementDelegate> ()
		{
			{BADGE_FIRST_TRIP.GetName (), BADGE_FIRST_TRIP_CHECK_DELEGATE},
			{BADGE_PERFECT_TRIP.GetName (), BADGE_PERFECT_TRIP_CHECK_DELEGATE},
			{BADGE_SAFETY_FIRST.GetName (), BADGE_SAFETY_FIRST_CHECK_DELEGATE},
			{BADGE_EFFICIENT.GetName (), BADGE_EFFICIENT_CHECK_DELEGATE},
			{BADGE_HIGH_ACHIEVER.GetName (), BADGE_HIGH_ACHIEVER_CHECK_DELEGATE},
			{BADGE_VETERAN.GetName (), BADGE_VETERAN_CHECK_DELEGATE},
			{BADGE_SELF_IMPROVEMENT.GetName (), BADGE_SELF_IMPROVEMENT_CHECK_DELEGATE},
			{BADGE_PERFECTIONIST.GetName (), BADGE_PERFECTIONIST_CHECK_DELEGATE},
			{BADGE_UNTOUCHABLE.GetName (), BADGE_UNTOUCHABLE_CHECK_DELEGATE}
		};

		public static Dictionary<string, UpdateAchievementDelegate> badgeUpdateDelegateMapping = new Dictionary<string, UpdateAchievementDelegate> ()
		{
			{BADGE_FIRST_TRIP.GetName (), BADGE_FIRST_TRIP_UPDATE_DELEGATE},
			{BADGE_PERFECT_TRIP.GetName (), BADGE_PERFECT_TRIP_UPDATE_DELEGATE},
			{BADGE_SAFETY_FIRST.GetName (), BADGE_SAFETY_FIRST_UPDATE_DELEGATE},
			{BADGE_EFFICIENT.GetName (), BADGE_EFFICIENT_UPDATE_DELEGATE},
			{BADGE_HIGH_ACHIEVER.GetName (), BADGE_HIGH_ACHIEVER_UPDATE_DELEGATE},
			{BADGE_VETERAN.GetName (), BADGE_VETERAN_UPDATE_DELEGATE},
			{BADGE_SELF_IMPROVEMENT.GetName (), BADGE_SELF_IMPROVEMENT_UPDATE_DELEGATE},
			{BADGE_PERFECTIONIST.GetName (), BADGE_PERFECTIONIST_UPDATE_DELEGATE},
			{BADGE_UNTOUCHABLE.GetName (), BADGE_UNTOUCHABLE_UPDATE_DELEGATE}
		};

		public static List<Badge> DEFAULT_BADGE_COLLECTION = new List<Badge> () {
			BADGE_FIRST_TRIP,
			BADGE_PERFECT_TRIP,
			BADGE_SAFETY_FIRST,
			BADGE_EFFICIENT,
			BADGE_HIGH_ACHIEVER,
			BADGE_VETERAN,
			BADGE_SELF_IMPROVEMENT,
			BADGE_PERFECTIONIST,
			BADGE_UNTOUCHABLE
		};

		private static AchievementManager _instance;

		private List<Badge> mBadgeCollection = new List<Badge> ();
		private UserBadgesRepository _userBadgeRepository;
		private StatisticsManager _statsManager;

		public static AchievementManager GetInstance ()
		{
			if (_instance == null) {
				_instance = new AchievementManager ();
			}
			return _instance;
		}

		public static void Uninitialize ()
		{
			_instance = null;
		}

		private AchievementManager ()
		{
			_userBadgeRepository = GamificationApp.GetInstance ().MyUserBadgesRepository;
			_statsManager = GamificationApp.GetInstance ().MyStatisticsManager;
			attachListeners ();
			SyncFromDb ();
		}

		private static Badge createBadge (string badgeName, string badgeDescription, int badgeDrawable)
		{
			return new Badge (badgeName, badgeDescription, badgeDrawable);
		}

		private void SyncFromDb ()
		{
			mBadgeCollection.Clear ();
			UserBadges data = _userBadgeRepository.GetUserBadges ();
			List<Badge> badges = (List<Badge>) JsonSerializationUtility.Deserialize (data.badgeCollectionData);
			foreach (Badge badge in badges) {
				mBadgeCollection.Add (badge);
			}
			if (mBadgeCollection.Count < DEFAULT_BADGE_COLLECTION.Count) {
				mBadgeCollection.AddRange (DEFAULT_BADGE_COLLECTION.GetRange (mBadgeCollection.Count, DEFAULT_BADGE_COLLECTION.Count - mBadgeCollection.Count));
			}
		}

		private void SyncToDb ()
		{
			string badgeCollectionJson = JsonSerializationUtility.Serialize (mBadgeCollection);
			_userBadgeRepository.UpdateBadges (badgeCollectionJson);
		}

		public Badge GetBadge (string name)
		{
			return mBadgeCollection.Find (x => x.GetName ().Equals (name));
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
				if (!badge.IsFullyUnlocked ()) {
					lockedBadges.Add (badge);
				}
			}
			return lockedBadges;
		}

		private void attachListeners ()
		{
			_statsManager.StatisticsUpdatedEvent += (object sender, EventArgs e) => {
				UpdateAchievements ();
				CheckAchievements ();
			};
		}

		public void UpdateAchievements ()
		{
			List<Badge> lockedBadges = GetLockedBadgeCollection ();
			foreach (Badge lockedBadge in lockedBadges) {
				lockedBadge.UpdateAchievement ();
			}
		}

		public void CheckAchievements ()
		{
			List<Badge> lockedBadges = GetLockedBadgeCollection ();
			foreach (Badge lockedBadge in lockedBadges) {
				lockedBadge.CheckAchievement ();
			}
			SyncToDb ();
		}
			
		private static bool CheckNullAchievement ()
		{
			return true;
		}

		private static void UpdateNullAchievement ()
		{
		}

		private static bool CheckFirstTripAchievement () 
		{
			return GamificationApp.GetInstance ().MyStatisticsManager.MyStats.totalTrips >= 1;
		}

		private static bool CheckPerfectTripAchievement ()
		{
			bool isPerfectTrip = false;
			TripDataModel latestTripRecord = GamificationApp.GetInstance ().MyTripHistoryManager.GetLatestRecord ();
			if (latestTripRecord != null) {
				isPerfectTrip = latestTripRecord.TripSafetyScore == 100 && latestTripRecord.TripEfficiencyScore == 100;
			}
			return isPerfectTrip;
		}

		private static bool CheckSafetyFirstAchievement ()
		{
			double totalDistance = GamificationApp.GetInstance ().MyStatisticsManager.MyStats.totalDistance;
			double safetyScore = GamificationApp.GetInstance ().MyStatisticsManager.MyStats.safetyScore;
			return totalDistance >= 1000 && safetyScore >= 90;
		}

		private static bool CheckEfficientAchievement ()
		{
			double totalDistance = GamificationApp.GetInstance ().MyStatisticsManager.MyStats.totalDistance;
			double efficiencyScore = GamificationApp.GetInstance ().MyStatisticsManager.MyStats.efficiencyScore;
			return totalDistance >= 1000 && efficiencyScore >= 90;
		}
	
		private static bool CheckHighAchieverAchievement () 
		{
			double totalDistance = GamificationApp.GetInstance ().MyStatisticsManager.MyStats.totalDistance;
			double overallScore = GamificationApp.GetInstance ().MyStatisticsManager.OverallScore;
			return totalDistance >= 1000 && overallScore >= 90;
		}

		private static bool CheckVeteranAchievement ()
		{
			double totalDistance = GamificationApp.GetInstance ().MyStatisticsManager.MyStats.totalDistance;
			double overallScore = GamificationApp.GetInstance ().MyStatisticsManager.OverallScore;
			return totalDistance >= 10000 && overallScore >= 50;
		}

		private static bool CheckSelfImprovementAchievement () 
		{
			int numRecords = BADGE_SELF_IMPROVEMENT_STREAK + 1;
			List<TripDataModel> tripRecords = GamificationApp.GetInstance ().MyTripHistoryManager.GetLatestRecords (numRecords);
			if (tripRecords.Count < numRecords+1) return false;
			double previousScore = 0;
			for (int count = 0; count < numRecords; count++) {
				TripDataModel tripRecord = tripRecords [count];
				double safetyScore = tripRecord.TripSafetyScore;
				double efficiencyScore = tripRecord.TripEfficiencyScore;
				double overallScore = ScoreCalculator.CalculateOverallScore (safetyScore, efficiencyScore);
				if (overallScore <= previousScore) return false;
				previousScore = overallScore;
			}
			return true;
		}

		private static bool CheckPerfectionistAchievement () 
		{
			List<TripDataModel> tripRecords = GamificationApp.GetInstance ().MyTripHistoryManager.GetLatestRecords (BADGE_PERFECTIONIST_STREAK);
			if (tripRecords.Count < BADGE_PERFECTIONIST_STREAK) return false;
			foreach (TripDataModel tripRecord in tripRecords) {
				double safetyScore = tripRecord.TripSafetyScore;
				double efficiencyScore = tripRecord.TripEfficiencyScore;
				if (safetyScore < 100 || efficiencyScore < 100) return false;
			}
			return true;
		}

		private static bool CheckUntouchableAchievement ()
		{
			Badge badge = GetInstance ().GetBadge (BADGE_UNTOUCHABLE_NAME);
			double? distanceWithoutAccident = badge.GetProperty (BADGE_UNTOUCHABLE_KM_PROP);
			return distanceWithoutAccident.HasValue && distanceWithoutAccident.Value >= BADGE_UNTOUCHABLE_STREAK;
		}

		private static void UpdateUntouchableAchievement ()
		{
			Badge badge = GetInstance ().GetBadge (BADGE_UNTOUCHABLE_NAME);
			TripDataModel tripRecord = GamificationApp.GetInstance ().MyTripHistoryManager.GetLatestRecord ();
			if (tripRecord == null)	return;
			if (tripRecord.AccidentEventMetric.Count > 0) {
				badge.SetProperty (BADGE_UNTOUCHABLE_KM_PROP, 0);
			} else {
				badge.IncrementProperty (BADGE_UNTOUCHABLE_KM_PROP, tripRecord.GetTripStats ().totalDistance);
			}
		}
	}
}

