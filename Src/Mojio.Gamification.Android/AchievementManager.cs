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
		private const string BADGE_FIRST_TRIP_NAME = "First Trip";
		private const string BADGE_PERFECT_TRIP_NAME = "Perfect Trip";
		private const string BADGE_SAFETY_FIRST_NAME = "Safety First";
		private const string BADGE_EFFICIENT_NAME = "Efficient";
		private const string BADGE_HIGH_ACHIEVER_NAME = "High Achiever";
		private const string BADGE_VETERAN_NAME = "Veteran";
		private const string BADGE_SELF_IMPROVEMENT_NAME = "Self-Improvement";
		private const string BADGE_PERFECTIONIST_NAME = "Perfectionist";
		private const string BADGE_UNTOUCHABLE_NAME = "Untouchable";
		private const string BADGE_TEST_BADGE_1_NAME = "Test Badge 1";
		private const string BADGE_TEST_BADGE_2_NAME = "Test Badge 2";
		private const string BADGE_TEST_BADGE_3_NAME = "Test Badge 3";

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
		private const string BADGE_TEST_BADGE_1_DESCRIPTION = "Test badge 1 description.";
		private const string BADGE_TEST_BADGE_2_DESCRIPTION = "Test badge 2 description.";
		private const string BADGE_TEST_BADGE_3_DESCRIPTION = "Test badge 3 description.";

		//Badges
		private static Badge BADGE_FIRST_TRIP = createBadge (BADGE_FIRST_TRIP_NAME, BADGE_FIRST_TRIP_DESCRIPTION, Resource.Drawable.Icon, Badge.BadgeType.NORMAL);
		private static Badge BADGE_PERFECT_TRIP = createBadge (BADGE_PERFECT_TRIP_NAME, BADGE_PERFECT_TRIP_DESCRIPTION, Resource.Drawable.Icon, Badge.BadgeType.COUNT);
		private static Badge BADGE_SAFETY_FIRST = createBadge (BADGE_SAFETY_FIRST_NAME, BADGE_SAFETY_FIRST_DESCRIPTION, Resource.Drawable.Icon, Badge.BadgeType.NORMAL);
		private static Badge BADGE_EFFICIENT = createBadge (BADGE_EFFICIENT_NAME, BADGE_EFFICIENT_DESCRIPTION, Resource.Drawable.Icon, Badge.BadgeType.NORMAL);
		private static Badge BADGE_HIGH_ACHIEVER = createBadge (BADGE_HIGH_ACHIEVER_NAME, BADGE_HIGH_ACHIEVER_DESCRIPTION, Resource.Drawable.Icon, Badge.BadgeType.NORMAL);
		private static Badge BADGE_VETERAN = createBadge (BADGE_VETERAN_NAME, BADGE_VETERAN_DESCRIPTION, Resource.Drawable.Icon, Badge.BadgeType.NORMAL);
		private static Badge BADGE_SELF_IMPROVEMENT = createBadge (BADGE_SELF_IMPROVEMENT_NAME, BADGE_SELF_IMPROVEMENT_DESCRIPTION, Resource.Drawable.Icon, Badge.BadgeType.LEVEL);
		private static Badge BADGE_PERFECTIONIST = createBadge (BADGE_PERFECTIONIST_NAME, BADGE_PERFECTIONIST_DESCRIPTION, Resource.Drawable.Icon, Badge.BadgeType.LEVEL);
		private static Badge BADGE_UNTOUCHABLE = createBadge (BADGE_UNTOUCHABLE_NAME, BADGE_UNTOUCHABLE_DESCRIPTION, Resource.Drawable.Icon, Badge.BadgeType.LEVEL);
		private static Badge BADGE_TEST_BADGE_1 = createBadge (BADGE_TEST_BADGE_1_NAME, BADGE_TEST_BADGE_1_DESCRIPTION, Resource.Drawable.Icon, Badge.BadgeType.NORMAL);
		private static Badge BADGE_TEST_BADGE_2 = createBadge (BADGE_TEST_BADGE_2_NAME, BADGE_TEST_BADGE_2_DESCRIPTION, Resource.Drawable.Icon, Badge.BadgeType.NORMAL);
		private static Badge BADGE_TEST_BADGE_3 = createBadge (BADGE_TEST_BADGE_3_NAME, BADGE_TEST_BADGE_3_DESCRIPTION, Resource.Drawable.Icon, Badge.BadgeType.NORMAL);

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
		private static CheckAchievementDelegate BADGE_TEST_BADGE_1_CHECK_DELEGATE = CheckNullAchievement;
		private static CheckAchievementDelegate BADGE_TEST_BADGE_2_CHECK_DELEGATE = CheckNullAchievement;
		private static CheckAchievementDelegate BADGE_TEST_BADGE_3_CHECK_DELEGATE = CheckNullAchievement;

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
		private static UpdateAchievementDelegate BADGE_TEST_BADGE_1_UPDATE_DELEGATE = UpdateNullAchievement;
		private static UpdateAchievementDelegate BADGE_TEST_BADGE_2_UPDATE_DELEGATE = UpdateNullAchievement;
		private static UpdateAchievementDelegate BADGE_TEST_BADGE_3_UPDATE_DELEGATE = UpdateNullAchievement;

		private const int BADGE_SELF_IMPROVEMENT_LEVEL_1_STREAK = 5;
		private const int BADGE_SELF_IMPROVEMENT_LEVEL_2_STREAK = 10;
		private const int BADGE_SELF_IMPROVEMENT_LEVEL_3_STREAK = 20;

		private const int BADGE_PERFECTIONIST_LEVEL_1_STREAK = 5;
		private const int BADGE_PERFECTIONIST_LEVEL_2_STREAK = 10;
		private const int BADGE_PERFECTIONIST_LEVEL_3_STREAK = 20;

		private const string BADGE_UNTOUCHABLE_KM_PROP = "distanceTravelled";
		private const int BADGE_UNTOUCHABLE_LEVEL_1_STREAK = 10000;
		private const int BADGE_UNTOUCHABLE_LEVEL_2_STREAK = 20000;
		private const int BADGE_UNTOUCHABLE_LEVEL_3_STREAK = 30000;

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
			{BADGE_UNTOUCHABLE.GetName (), BADGE_UNTOUCHABLE_CHECK_DELEGATE},
			{BADGE_TEST_BADGE_1.GetName (), BADGE_TEST_BADGE_1_CHECK_DELEGATE},
			{BADGE_TEST_BADGE_2.GetName (), BADGE_TEST_BADGE_2_CHECK_DELEGATE},
			{BADGE_TEST_BADGE_3.GetName (), BADGE_TEST_BADGE_3_CHECK_DELEGATE}
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
			{BADGE_UNTOUCHABLE.GetName (), BADGE_UNTOUCHABLE_UPDATE_DELEGATE},
			{BADGE_TEST_BADGE_1.GetName (), BADGE_TEST_BADGE_1_UPDATE_DELEGATE},
			{BADGE_TEST_BADGE_2.GetName (), BADGE_TEST_BADGE_2_UPDATE_DELEGATE},
			{BADGE_TEST_BADGE_3.GetName (), BADGE_TEST_BADGE_3_UPDATE_DELEGATE}
		};

		private static AchievementManager _instance;

		private List<Badge> mBadgeCollection = new List<Badge> ();
		private UserBadgeRepository _userBadgeRepository;
		private StatisticsManager _statsManager;

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

		private List<Badge> createBadgeCollection ()
		{
			List<Badge> badgeCollection = new List<Badge> ();
			badgeCollection.Add (BADGE_FIRST_TRIP);
			badgeCollection.Add (BADGE_PERFECT_TRIP);
			badgeCollection.Add (BADGE_SAFETY_FIRST);
			badgeCollection.Add (BADGE_EFFICIENT);
			badgeCollection.Add (BADGE_HIGH_ACHIEVER);
			badgeCollection.Add (BADGE_VETERAN);
			badgeCollection.Add (BADGE_SELF_IMPROVEMENT);
			badgeCollection.Add (BADGE_PERFECTIONIST);
			badgeCollection.Add (BADGE_UNTOUCHABLE);
			badgeCollection.Add (BADGE_TEST_BADGE_1);
			badgeCollection.Add (BADGE_TEST_BADGE_2);
			badgeCollection.Add (BADGE_TEST_BADGE_3);
			return badgeCollection;
		}

		private static Badge createBadge (string badgeName, string badgeDescription, int badgeDrawable, Badge.BadgeType badgeType)
		{
			Badge.BadgeType type = badgeType;
			switch (type) {
			case Badge.BadgeType.NORMAL: 	return new Badge (badgeName, badgeDescription, badgeDrawable);
			case Badge.BadgeType.COUNT: 	return new CountBadge (badgeName, badgeDescription, badgeDrawable);
			case Badge.BadgeType.LEVEL:		return new LevelBadge (badgeName, badgeDescription, badgeDrawable);
			default: 						throw new ArgumentException (String.Format ("Invalid badge type - {0}.", type.ToString ()));
			}
		}

		private void SyncFromDb ()
		{
			mBadgeCollection.Clear ();
			List<UserBadge> data = _userBadgeRepository.GetUserBadges ();
			if (data.Count == 0) {
				mBadgeCollection = createBadgeCollection ();
				SyncToDb ();
			} else {
				foreach (UserBadge badge in data) {
					mBadgeCollection.Add (Badge.Deserialize (badge.badgeData));
				}
			}
		}

		private void SyncToDb ()
		{
			List<UserBadge> collection = new List<UserBadge> ();
			foreach (Badge badge in mBadgeCollection) {
				UserBadge userBadge = new UserBadge ();
				userBadge.badgeName = badge.GetName ();
				userBadge.badgeData = Badge.Serialize (badge);
				collection.Add (userBadge);
			}
			_userBadgeRepository.UpdateBadges (collection);
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
			List<TripRecord> tripRecords = GamificationApp.GetInstance ().MyTripRecordRepository.GetRecords ();
			if (tripRecords.Count > 0) {
				TripDataModel tripRecord = TripDataModel.Deserialize (tripRecords[0].tripData);
				isPerfectTrip = tripRecord.TripSafetyScore == 100 && tripRecord.TripEfficiencyScore == 100;
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
			int records = 0;
			LevelBadge badge = (LevelBadge) GetInstance ().GetBadge (BADGE_SELF_IMPROVEMENT_NAME);
			switch (badge.GetLevel ()) {
			case 0: records = BADGE_SELF_IMPROVEMENT_LEVEL_1_STREAK; 	break;
			case 1:	records = BADGE_SELF_IMPROVEMENT_LEVEL_2_STREAK;	break;
			case 2:	records = BADGE_SELF_IMPROVEMENT_LEVEL_3_STREAK;	break;
			}

			List<TripRecord> tripRecords = GamificationApp.GetInstance ().MyTripRecordRepository.GetRecords ();
			if (tripRecords.Count < records) return false;
			double previousScore = 0;
			for (int count = 0; count < records; count++) {
				TripDataModel tripRecord = TripDataModel.Deserialize (tripRecords[count].tripData);
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
			int records = 0;
			LevelBadge badge = (LevelBadge) GetInstance ().GetBadge (BADGE_PERFECTIONIST_NAME);
			switch (badge.GetLevel ()) {
			case 0: records = BADGE_PERFECTIONIST_LEVEL_1_STREAK; 	break;
			case 1:	records = BADGE_PERFECTIONIST_LEVEL_2_STREAK;	break;
			case 2:	records = BADGE_PERFECTIONIST_LEVEL_3_STREAK;	break;
			}

			List<TripRecord> tripRecords = GamificationApp.GetInstance ().MyTripRecordRepository.GetRecords ();
			if (tripRecords.Count < records) return false;
			for (int count = 0; count < records; count++) {
				TripDataModel tripRecord = TripDataModel.Deserialize (tripRecords[count].tripData);
				double safetyScore = tripRecord.TripSafetyScore;
				double efficiencyScore = tripRecord.TripEfficiencyScore;
				if (safetyScore < 100 || efficiencyScore < 100) return false;
			}
			return true;
		}

		private static bool CheckUntouchableAchievement ()
		{
			LevelBadge badge = (LevelBadge) GetInstance ().GetBadge (BADGE_UNTOUCHABLE_NAME);
			double threshold = 0;
			switch (badge.GetLevel ()) {
			case 0: threshold = BADGE_UNTOUCHABLE_LEVEL_1_STREAK; 	break;
			case 1:	threshold = BADGE_UNTOUCHABLE_LEVEL_2_STREAK;	break;
			case 2:	threshold = BADGE_UNTOUCHABLE_LEVEL_3_STREAK;	break;
			}

			double? distanceWithoutAccident = badge.GetProperty (BADGE_UNTOUCHABLE_KM_PROP);
			return distanceWithoutAccident.HasValue && distanceWithoutAccident.Value >= threshold;
		}

		private static void UpdateUntouchableAchievement ()
		{
			LevelBadge badge = (LevelBadge) GetInstance ().GetBadge (BADGE_UNTOUCHABLE_NAME);
			TripRecord tripRecord = GamificationApp.GetInstance ().MyTripRecordRepository.GetLatestRecord ();
			if (tripRecord == null)
				return;
			TripDataModel tripData = TripDataModel.Deserialize (tripRecord.tripData);
			if (tripData.AccidentEventMetric.Count > 0) {
				badge.SetProperty (BADGE_UNTOUCHABLE_KM_PROP, 0);
			} else {
				badge.IncrementProperty (BADGE_UNTOUCHABLE_KM_PROP, tripData.GetTripStats ().totalDistance);
			}
		}
	}
}

