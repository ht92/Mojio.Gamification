using System;
using Android.Content;
using SQLite;
using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	public class UserStatsRepository
	{
		private DataManagerHelper _helper;

		public UserStatsRepository(Context context)
		{
			_helper = new DataManagerHelper (context);
		}

		public long AddUserStats (UserStats addUser)
		{
			using (var db = new SQLiteConnection (_helper.WritableDatabase.Path)) 
			{
				return db.Insert (addUser);	
			}
		}

		public long DeleteUserStats (UserStats deleteUser)
		{
			using (var db = new SQLiteConnection (_helper.WritableDatabase.Path)) 
			{
				return db.Delete (deleteUser);	
			}
		}

		public long UpdateUserStats (UserStats updateUser)
		{
			using (var db = new SQLiteConnection (_helper.WritableDatabase.Path)) 
			{
				return db.Update (updateUser);	
			}
		}

		public UserStats GetUserStats()
		{
			using (var database = new SQLiteConnection (_helper.ReadableDatabase.Path)) 
			{
				return database.Table<UserStats> ().ElementAt (0);
			}
		}

		public double GetUserStats(UserStatType type) 
		{
			using (var database = new SQLiteConnection (_helper.ReadableDatabase.Path)) 
			{
				switch (type) {
				case UserStatType.TOTAL_DISTANCE: return database.Table<UserStats> ().ElementAt (0).totalDistance;
				case UserStatType.TOTAL_DURATION: return database.Table<UserStats> ().ElementAt (0).totalDuration;
				case UserStatType.OVERALL_SCORE: return database.Table<UserStats> ().ElementAt (0).overallScore;
				case UserStatType.NUM_SPEEDING: return database.Table<UserStats> ().ElementAt (0).numSpeeding;
				case UserStatType.DISTANCE_SPEEDING: return database.Table<UserStats> ().ElementAt (0).distanceSpeeding;
				case UserStatType.NUM_HARSH_EVENTS: return database.Table<UserStats> ().ElementAt (0).numHarshEvents;
				case UserStatType.TOTAL_IDLE_TIME: return database.Table<UserStats> ().ElementAt (0).totalIdleTime;
				case UserStatType.TOTAL_FUEL_CONSUMPTION: return database.Table<UserStats> ().ElementAt (0).totalFuelConsumption;
				default: return -1;
				}
			}
		}

		public enum UserStatType {
			TOTAL_DISTANCE,
			TOTAL_DURATION,
			OVERALL_SCORE,
			NUM_SPEEDING,
			DISTANCE_SPEEDING,
			NUM_HARSH_EVENTS,
			TOTAL_IDLE_TIME,
			TOTAL_FUEL_CONSUMPTION
		};
	}
}

