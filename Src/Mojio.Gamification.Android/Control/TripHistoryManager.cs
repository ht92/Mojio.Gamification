using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	public class TripHistoryManager
	{
		private static TripHistoryManager _instance;
		private const int TRIP_HISTORY_MAX_COUNT = 30;

		private List<TripDataModel> mTripRecords = new List<TripDataModel> ();
		private UserTripRecordsRepository _userTripRecordsRepository;
		private StatisticsManager _statsManager;

		public static TripHistoryManager GetInstance ()
		{
			if (_instance == null) {
				_instance = new TripHistoryManager ();
			}
			return _instance;
		}

		private TripHistoryManager ()
		{
			_userTripRecordsRepository = GamificationApp.GetInstance ().MyUserTripRecordsRepository;
			_statsManager = GamificationApp.GetInstance ().MyStatisticsManager;
			attachListeners ();
			SyncFromDb ();
		}

		public void AddRecord (TripDataModel tripRecord)
		{
			if (mTripRecords.Count > TRIP_HISTORY_MAX_COUNT) {
				mTripRecords.RemoveAt (0);
			}
			mTripRecords.Add (tripRecord);
			SyncToDb ();
		}

		public List<TripDataModel> GetRecords ()
		{
			return mTripRecords;
		}

		public TripDataModel GetLatestRecord ()
		{
			List<TripDataModel> latestRecords = GetLatestRecords (1);
			if (latestRecords.Count > 0) {
				return latestRecords [0];
			} else {
				return null;
			}
		}

		public List<TripDataModel> GetLatestRecords (int numRecords)
		{
			List<TripDataModel> latestRecords = new List<TripDataModel> ();
			int count = mTripRecords.Count;
			if (count > 0) {
				int startingIndex = Math.Max (count - numRecords, 0);
				int range = count > numRecords ? numRecords : count;
				latestRecords.AddRange (mTripRecords.GetRange (startingIndex, range));
			}
			return latestRecords;
		}

		private void attachListeners ()
		{
			_statsManager.TripsAddedEvent += (object sender, StatisticsManager.TripsAddedEventArgs e) => AddRecord (e.TripData);
		}

		private void SyncFromDb ()
		{
			mTripRecords.Clear ();
			UserTripRecords data = _userTripRecordsRepository.GetTripRecords ();
			List<TripDataModel> tripRecords = (List<TripDataModel>) JsonSerializationUtility.Deserialize (data.tripHistoryData);
			foreach (TripDataModel tripRecord in tripRecords) {
				mTripRecords.Add (tripRecord);
			}
		}

		private void SyncToDb ()
		{
			string tripRecordsJson = JsonSerializationUtility.Serialize (mTripRecords);
			_userTripRecordsRepository.UpdateTripRecords (tripRecordsJson);
		}
	}
}

