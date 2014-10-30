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
							totalDistance			DOUBLE NOT NULL,
							totalDuration			INTEGER NOT NULL,
							overallScore			INTEGER NOT NULL,	
							numSpeeding				INTEGER NOT NULL,
							distanceSpeeding		DOUBLE NOT NULL,
							numHarshEvents			INTEGER NOT NULL,
							totalIdleTime			INTEGER NOT NULL,
							totalFuelConsumption	DOUBLE NOT NULL)");
			db.ExecSQL (@"INSERT INTO UserStats VALUES (0, 0.00, 0, 0, 0, 0.00, 0, 0, 0.00)");
		}

		public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
		{
			db.ExecSQL (@"DROP TABLE IF EXISTS UserStats");
			OnCreate (db);
		}

	}
}

