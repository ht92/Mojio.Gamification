using SQLite;

namespace Mojio.Gamification.Core
{
	public class TripRecord
	{
		[PrimaryKey]
		public double tripTimestamp { get; set; }
		public string tripData { get; set; }

		public static TripRecord Create (string json)
		{
			TripRecord record = new TripRecord ();
			record.tripTimestamp = DateTimeUtility.GetCurrentTimestamp ();
			record.tripData = json;
			return record;
		}
	}
}

