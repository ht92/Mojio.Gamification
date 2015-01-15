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

		public DataManagerHelper (Context context)
			: base (context, dbName, null, dbVersion)
		{
		}

		public override void OnCreate(SQLiteDatabase db)
		{
			db.ExecSQL (@"
						CREATE TABLE IF NOT EXISTS UserStats (
							uid						INTEGER PRIMARY KEY AUTOINCREMENT,
							totalTrips				INTEGER NOT NULL,
							totalDistance			DOUBLE NOT NULL,
							totalDuration			DOUBLE NOT NULL,
							totalHardAccelerations  INTEGER NOT NULL,
							totalHardBrakes			INTEGER NOT NULL,
							totalHardLefts			INTEGER NOT NULL,
							totalHardRights			INTEGER NOT NULL,
							totalIdleEvents			INTEGER NOT NULL,
							totalFuelConsumption    DOUBLE NOT NULL,
							safetyScore				DOUBLE NOT NULL,
							efficiencyScore			DOUBLE NOT NULL)");
			db.ExecSQL (@"INSERT INTO UserStats VALUES (0, 0, 0.00, 0.00, 0, 0, 0, 0, 0, 0.00, 0.00, 0.00)");
		}

		public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
		{
			db.ExecSQL (@"DROP TABLE IF EXISTS UserStats");
			OnCreate (db);
		}
	}
}

