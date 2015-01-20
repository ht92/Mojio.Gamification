using System;
using System.Collections.Generic;
using Android.App;
using Mojio.Events;


using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	public class StatisticsManager
	{
		public event EventHandler StatisticsUpdatedEvent;

		public UserStats MyStats { get; private set; } 
		public double OverallScore;

		private UserStatsRepository _userStatsRepository;
		private static StatisticsManager _instance;

		public static StatisticsManager GetInstance()
		{
			if (_instance == null) {
				_instance = new StatisticsManager ();
			}
			return _instance;
		}

		public void AddTrip (Trip trip, IList<Event> events)
		{
			if (TripDataModel.IsTripValid (trip)) {
				RecalculateScore (trip, events);
			} else {
				Logger.GetInstance ().Warning (String.Format ("Invalid trip detected - {0}. Discarding...", trip.Id.ToString ()));
			}
		}

		private StatisticsManager () 
		{
			//initialize with the current stats
			_userStatsRepository = GamificationApp.GetInstance ().MyUserStatsRepository;
			attachListeners ();
			MyStats = _userStatsRepository.GetUserStats ();
			setOverallScore ();
		}

		private void setOverallScore()
		{
			OverallScore = ScoreCalculator.CalculateOverallScore (new List<double> {
				MyStats.safetyScore,
				MyStats.efficiencyScore
			});
		}

		private void RecalculateScore (Trip trip, IList<Event> events)
		{
			TripDataModel tripData = new TripDataModel (trip, events);
			UserStats tripStats = tripData.GetTripStats ();
			UserStats newStats = UserStats.SumStats (MyStats, tripStats);
			_userStatsRepository.UpdateUserStats (newStats);
		}

		private void attachListeners ()
		{
			_userStatsRepository.UserStatsUpdatedEvent += (object sender, EventArgs e) => syncWithDatabase ();
		}
			
		private void syncWithDatabase ()
		{
			MyStats = _userStatsRepository.GetUserStats ();
			setOverallScore ();
			OnStatisticsUpdatedEvent (EventArgs.Empty);
		}

		protected void OnStatisticsUpdatedEvent (EventArgs e)
		{
			StatisticsUpdatedEvent (this, e);
		}
	}
}

