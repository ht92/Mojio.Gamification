using System;
using SQLite;
using Android.Content;
using Android.Database.Sqlite;

namespace Mojio.Gamification.Android
{
	public class DataManagerHelper : SQLiteOpenHelper
	{

		private const string dbName = "myDatabase";
		private const int dbVersion = 3;

		private static DataManagerHelper _instance;

		public static DataManagerHelper GetInstance ()
		{
			if (_instance == null) {
				_instance = new DataManagerHelper (GamificationApp.GetInstance ());
			}
			return _instance;
		}

		private DataManagerHelper (Context context)
			: base (context, dbName, null, dbVersion)
		{
		}

		public override void OnCreate(SQLiteDatabase db)
		{
			db.ExecSQL (@"
						CREATE TABLE IF NOT EXISTS UserStats (
							uid						VARCHAR PRIMARY KEY,
							totalTrips				INTEGER NOT NULL,
							totalDistance			DOUBLE NOT NULL,
							totalDuration			DOUBLE NOT NULL,
							totalHardAccelerations  INTEGER NOT NULL,
							totalHardBrakes			INTEGER NOT NULL,
							totalHardLefts			INTEGER NOT NULL,
							totalHardRights			INTEGER NOT NULL,
							totalAccidents			INTEGER NOT NULL,
							totalFuelConsumption    DOUBLE NOT NULL,
							safetyScore				DOUBLE NOT NULL,
							efficiencyScore			DOUBLE NOT NULL)");
			db.ExecSQL (@"
						CREATE TABLE IF NOT EXISTS UserBadges (
							uid						VARCHAR PRIMARY KEY,
							badgeCollectionData		VARCHAR NOT NULL)");
			db.ExecSQL (@"
						CREATE TABLE IF NOT EXISTS UserTripRecords (
							uid							VARCHAR PRIMARY KEY,
							tripHistoryData 			VARCHAR NOT NULL)");
		}

		public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
		{
			db.ExecSQL (@"DROP TABLE IF EXISTS UserStats");
			db.ExecSQL (@"DROP TABLE IF EXISTS UserBadge");
			db.ExecSQL (@"DROP TABLE IF EXISTS TripRecord");
			OnCreate (db);
		}
	}
}

