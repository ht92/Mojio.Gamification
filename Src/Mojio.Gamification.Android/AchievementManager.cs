﻿using System;
using System.Collections.Generic;
using Mojio.Gamification.Core;
using Mojio.Gamification.Android;
using Newtonsoft.Json;

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
			bool isPerfectTrip = false;
			List<TripRecord> tripRecords = GamificationApp.GetInstance ().MyTripRecordRepository.GetRecords ();
			if (tripRecords.Count > 0) {
				TripDataModel tripRecord = TripDataModel.Deserialize (tripRecords[0].tripData);
				isPerfectTrip = tripRecord.TripSafetyScore == 100 && tripRecord.TripEfficiencyScore == 100;
			}
			return isPerfectTrip;
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

		public static bool CheckVeteranAchievement ()
		{
			double totalDistance = GamificationApp.GetInstance ().MyStatisticsManager.MyStats.totalDistance;
			double overallScore = GamificationApp.GetInstance ().MyStatisticsManager.OverallScore;
			return totalDistance >= 10000 && overallScore >= 50;
		}

		public static bool CheckSelfImprovementAchievement () 
		{
			List<TripRecord> tripRecords = GamificationApp.GetInstance ().MyTripRecordRepository.GetRecords ();

			if (tripRecords.Count < 5) return false;

			double previousScore = 0;
			for (int count = 0; count < 5; count++) {
				TripDataModel tripRecord = TripDataModel.Deserialize (tripRecords[count].tripData);
				double safetyScore = tripRecord.TripSafetyScore;
				double efficiencyScore = tripRecord.TripEfficiencyScore;
				double overallScore = ScoreCalculator.CalculateOverallScore (safetyScore, efficiencyScore);
				if (overallScore <= previousScore) return false;
				previousScore = overallScore;
			}
			return true;
		}

		public static bool CheckPerfectionistAchievement () 
		{
			List<TripRecord> tripRecords = GamificationApp.GetInstance ().MyTripRecordRepository.GetRecords ();

			if (tripRecords.Count < 5) return false;

			for (int count = 0; count < 5; count++) {
				TripDataModel tripRecord = TripDataModel.Deserialize (tripRecords[count].tripData);
				double safetyScore = tripRecord.TripSafetyScore;
				double efficiencyScore = tripRecord.TripEfficiencyScore;
				if (safetyScore < 100 || efficiencyScore < 100) return false;
			}
			return true;
		}

		public static bool CheckUntouchableAchievement ()
		{
			List<TripRecord> tripRecords = GamificationApp.GetInstance ().MyTripRecordRepository.GetRecords ();

			if (tripRecords.Count < 5) return false;

			for (int count = 0; count < 5; count++) {
				TripDataModel tripRecord = TripDataModel.Deserialize (tripRecords[count].tripData);
				if (tripRecord.AccidentEventMetric.Count > 0) return false;
			}
			return true;
		}
	}
}

