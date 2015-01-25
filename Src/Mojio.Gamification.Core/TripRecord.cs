using System;
using SQLite;

namespace Mojio.Gamification.Core
{
	public class TripRecord
	{
		[PrimaryKey]
		public double tripTimestamp { get; set; }
		public string tripData { get; set; }

		public static DateTime UTC_OFFSET = new DateTime(1970, 1, 1);
		public static TripRecord Create (string json)
		{
			TripRecord record = new TripRecord ();
			record.tripTimestamp = GetCurrentTimestamp ();
			record.tripData = json;
			return record;
		}

		public static double GetCurrentTimestamp ()
		{
			return (DateTime.Now.Subtract (UTC_OFFSET)).TotalSeconds;
		}

		public static double GetPastTimestamp (int daysBack)
		{
			return (DateTime.Now.AddDays (daysBack * -1).Subtract (TripRecord.UTC_OFFSET)).TotalSeconds;
		}
	}
}

