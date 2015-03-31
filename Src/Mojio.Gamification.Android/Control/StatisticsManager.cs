using System;
using System.Collections.Generic;
using Mojio.Events;

using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	public class StatisticsManager
	{
		public event EventHandler<TripsAddedEventArgs> TripsAddedEvent;
		public event EventHandler StatisticsUpdatedEvent;

		public UserStatsModel MyStats { get; private set; } 
		public double OverallScore;

		private ConnectionService mConnectionService;
		private UserStatsRepository mUserStatsRepository;
		private static StatisticsManager _instance;

		public static StatisticsManager GetInstance()
		{
			if (_instance == null) {
				_instance = new StatisticsManager ();
			}
			return _instance;
		}

		public static void Uninitialize ()
		{
			_instance = null;
		}

		public void AddTrip (Trip trip, IList<Event> events)
		{
			if (TripDataModel.IsTripValid (trip)) {
				TripDataModel tripData = new TripDataModel (trip, events);
				OnTripsAddedEvent (new TripsAddedEventArgs (tripData));
				RecalculateScore (tripData);
			} else {
				Logger.GetInstance ().Warning (String.Format ("Invalid trip detected - {0}. Discarding...", trip.Id.ToString ()));
			}
		}

		private StatisticsManager () 
		{
			//initialize with the current stats
			mUserStatsRepository = GamificationApp.GetInstance ().MyUserStatsRepository;
			mConnectionService = GamificationApp.GetInstance ().MyConnectionService;
			attachListeners ();
			initializeScore ();
		}

		private void setOverallScore()
		{
			OverallScore = ScoreCalculator.CalculateOverallScore (MyStats.safetyScore, MyStats.efficiencyScore);
		}

		private void RecalculateScore (TripDataModel tripData)
		{
			UserStatsModel tripStats = tripData.GetTripStats ();
			UserStatsModel newStats = UserStatsModel.SumStatsModel (MyStats, tripStats);
			syncToDatabase (newStats);
			GamificationApp.GetInstance ().MyNotificationService.IssueTripNotification (tripData);
		}

		private void attachListeners ()
		{
			mUserStatsRepository.UserStatsUpdatedEvent += (sender, e) => syncFromDatabase ();
		}

		private void initializeScore ()
		{
			UserStats userStats = mUserStatsRepository.GetUserStats ();
			MyStats = (UserStatsModel) JsonSerializationUtility.Deserialize (userStats.userStatsData);
			setOverallScore ();
		}

		private void syncFromDatabase ()
		{			
			UserStats userStats = mUserStatsRepository.GetUserStats ();
			MyStats = (UserStatsModel) JsonSerializationUtility.Deserialize (userStats.userStatsData);
			setOverallScore ();
			OnStatisticsUpdatedEvent (EventArgs.Empty);
		}

		private void syncToDatabase (UserStatsModel newStats)
		{
			UserStats userStats = UserStats.CreateStats (mConnectionService.CurrentUserName);
			userStats.userStatsData = JsonSerializationUtility.Serialize (newStats);
			mUserStatsRepository.UpdateUserStats (userStats);
		}

		protected void OnTripsAddedEvent (TripsAddedEventArgs e)
		{
			TripsAddedEvent (this, e);
		}

		protected void OnStatisticsUpdatedEvent (EventArgs e)
		{
			StatisticsUpdatedEvent (this, e);
		}

		public class TripsAddedEventArgs : EventArgs
		{
			public TripDataModel TripData { get; set; }
			public TripsAddedEventArgs (TripDataModel tripData) 
			{
				TripData = tripData;
			}
		}
	}
}

