using System;
using System.Collections.Generic;
using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	public class TripHistoryManager
	{
		private static TripHistoryManager _instance;
		private const int TRIP_HISTORY_MAX_COUNT = 30;

		private SortedList<DateTime,TripDataModel> mTripRecords;
		private readonly UserTripRecordsRepository mUserTripRecordsRepository;
		private StatisticsManager mStatsManager;

		public static TripHistoryManager GetInstance ()
		{
			if (_instance == null) {
				_instance = new TripHistoryManager ();
			}
			return _instance;
		}

		private TripHistoryManager ()
		{
			mTripRecords = CreateNewHistory ();
			mUserTripRecordsRepository = GamificationApp.GetInstance ().MyUserTripRecordsRepository;
			mStatsManager = GamificationApp.GetInstance ().MyStatisticsManager;
			attachListeners ();
			SyncFromDb ();
		}

		public static SortedList<DateTime,TripDataModel> CreateNewHistory ()
		{
			return new SortedList<DateTime,TripDataModel> (TRIP_HISTORY_MAX_COUNT, new ByDateTimeComparer ()); 
		}

		public void AddRecord (TripDataModel tripRecord)
		{
			int numRecords = mTripRecords.Count;
			if (numRecords >= TRIP_HISTORY_MAX_COUNT) {
				mTripRecords.RemoveAt(numRecords-1);
			}
			try {
				mTripRecords.Add (tripRecord.MyTrip.StartTime, tripRecord);
			} catch (ArgumentException e) {
				Logger.GetInstance ().Warning (e.Message);
			}
			SyncToDb ();
		}

		public IList<TripDataModel> GetRecords ()
		{
			return mTripRecords.Values;
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
				int range = count > numRecords ? numRecords : count;
				for (int i = 0; i < range; i++) {
					latestRecords.Add (mTripRecords.Values [i]);
				}
			}
			return latestRecords;
		}

		private void attachListeners ()
		{
			mStatsManager.TripsAddedEvent += (sender, e) => AddRecord (e.TripData);
		}

		private void SyncFromDb ()
		{
			mTripRecords.Clear ();
			UserTripRecords data = mUserTripRecordsRepository.GetTripRecords ();
			var deserialized = (SortedList<DateTime,TripDataModel>) JsonSerializationUtility.Deserialize (data.tripHistoryData);
			foreach (TripDataModel trip in deserialized.Values) {
				mTripRecords.Add (trip.MyTrip.StartTime, trip);
			}
		}

		private void SyncToDb ()
		{
			string tripRecordsJson = JsonSerializationUtility.Serialize (mTripRecords);
			mUserTripRecordsRepository.UpdateTripRecords (tripRecordsJson);
		}

		/*
		 * Comparer to sort list by DateTime in descending order.
		 */
		public class ByDateTimeComparer : IComparer<DateTime>
		{
			public int Compare (DateTime x, DateTime y)
			{
				return Comparer<DateTime>.Default.Compare (y, x);
			}
		}
	}
}

