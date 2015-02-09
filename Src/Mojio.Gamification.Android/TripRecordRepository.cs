using System.Collections.Generic;
using SQLite;
using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	public class TripRecordRepository
	{
		private readonly DataManagerHelper _helper;
		private static TripRecordRepository _instance;

		public static TripRecordRepository GetInstance ()
		{
			if (_instance == null) {
				_instance = new TripRecordRepository ();
			}
			_instance.clearOldEntries ();
			return _instance;
		}

		private TripRecordRepository ()
		{
			_helper = new DataManagerHelper (GamificationApp.GetInstance ());
		}

		public long AddRecord (TripRecord tripRecord)
		{
			using (var db = new SQLiteConnection (_helper.WritableDatabase.Path)) 
			{
				long count = db.Insert (tripRecord);	
				return count;
			}
		}

		public long RemoveRecord (TripRecord tripRecord)
		{
			using (var db = new SQLiteConnection (_helper.WritableDatabase.Path)) 
			{
				long count = db.Delete (tripRecord);
				return count;
			}
		}

		public long Clear ()
		{
			using (var db = new SQLiteConnection (_helper.WritableDatabase.Path)) 
			{
				long count = db.DeleteAll<TripRecord> ();	
				return count;
			}
		}

		public List<TripRecord> GetRecords ()
		{
			using (var db = new SQLiteConnection (_helper.ReadableDatabase.Path)) 
			{
				List<TripRecord> records = new List<TripRecord> ();
				int count = db.Table<TripRecord> ().Count ();
				for (int index = 0; index < count; index++) {
					records.Add (db.Table<TripRecord> ().ElementAt (index));
				}
				records.Reverse ();
				return records;
			}
		}

		public TripRecord GetLatestRecord ()
		{
			using (var db = new SQLiteConnection (_helper.ReadableDatabase.Path)) 
			{
				TripRecord latestRecord = null;
				int count = db.Table<TripRecord> ().Count ();
				if (count > 0) {
					latestRecord = db.Table<TripRecord> ().ElementAt (count - 1);
				}
				return latestRecord;
			}
		}

		private long clearOldEntries ()
		{
			using (var db = new SQLiteConnection (_helper.WritableDatabase.Path)) 
			{
				long entriesModified = 0;
				if (db.Table<TripRecord> ().Count () > 30) {
					double currentTimestamp = DateTimeUtility.GetPastTimestamp (7);
					SQLiteCommand command = db.CreateCommand ("DELETE FROM TripRecord WHERE tripTimestamp <= ?", currentTimestamp);
					entriesModified = command.ExecuteNonQuery ();
				}
				return entriesModified;
			}
		}
	}
}

