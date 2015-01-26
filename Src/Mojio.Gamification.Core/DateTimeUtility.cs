using System;

namespace Mojio.Gamification.Core
{
	public class DateTimeUtility
	{
		public static DateTime UTC_OFFSET = new DateTime(1970, 1, 1);
		public static string FORMAT_MMMM_DD_YYYY_H_MM_TT = "MMMM dd, yyyy h:mm tt";

		public static double GetCurrentTimestamp ()
		{
			return (DateTime.Now.Subtract (UTC_OFFSET)).TotalSeconds;
		}

		public static double GetPastTimestamp (int daysBack)
		{
			return (DateTime.Now.AddDays (daysBack * -1).Subtract (UTC_OFFSET)).TotalSeconds;
		}
	}
}

