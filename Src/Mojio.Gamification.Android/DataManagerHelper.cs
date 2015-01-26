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
							totalAccidents			INTEGER NOT NULL,
							totalIdleEvents			INTEGER NOT NULL,
							totalFuelConsumption    DOUBLE NOT NULL,
							safetyScore				DOUBLE NOT NULL,
							efficiencyScore			DOUBLE NOT NULL)");
			db.ExecSQL (@"INSERT INTO UserStats VALUES (0, 0, 0.00, 0.00, 0, 0, 0, 0, 0, 0, 0.00, 0.00, 0.00)");

			db.ExecSQL (@"
						CREATE TABLE IF NOT EXISTS UserBadge (
							badgeId					INTEGER PRIMARY KEY AUTOINCREMENT,
							badgeName				VARCHAR NOT NULL,
							badgeType 				INTEGER NOT NULL,
							badgeLevel				INTEGER NOT NULL,
							badgeMaxLevel			INTEGER)");
			db.ExecSQL (@"INSERT INTO UserBadge (badgeId, badgeName, badgeType, badgeLevel, badgeMaxLevel) VALUES 
							(0, 'First Trip', 0, 0, 1),
							(1, 'Perfect Trip', 1, 0, null),
							(2, 'Safety First', 0, 0, 1),
							(3, 'Efficient', 0, 0, 1),
							(4, 'High Achiever', 0, 0, 1),
							(5, 'Veteran', 0, 0, 1),
							(6, 'Self-Improvement', 2, 0, 3),
							(7, 'Perfectionist', 2, 0, 3),
							(8, 'Untouchable', 2, 0, 3),
							(9, 'TestBadge1', 0, 1, 1),
							(10, 'TestBadge2', 0, 1, 1),
							(11, 'TestBadge3', 0, 1, 1)");
			db.ExecSQL (@"
						CREATE TABLE IF NOT EXISTS TripRecord (
							tripTimestamp				DOUBLE PRIMARY KEY,
							tripData					VARCHAR NOT NULL)");
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

