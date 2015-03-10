using System;

namespace Mojio.Gamification.Core
{
	public class ValueUnit 
	{
		public static readonly ValueUnit NULL_UNIT = new ValueUnit(String.Empty);
		public static readonly ValueUnit DISTANCE_M = new ValueUnit("meters", "m");
		public static readonly ValueUnit DISTANCE_KM = new ValueUnit("kilometers", "km");
		public static readonly ValueUnit DISTANCE_100KM = new ValueUnit("100 kilometers", "100km");
		public static readonly ValueUnit TIME_SECONDS = new ValueUnit("seconds", "s");
		public static readonly ValueUnit TIME_MINUTES = new ValueUnit("minutes", "min");
		public static readonly ValueUnit TIME_HOURS = new ValueUnit("hours", "hr");
		public static readonly ValueUnit LITRE = new ValueUnit("litre", "L");
		public static readonly ValueUnit PERCENTAGE = new ValueUnit("%");

		public readonly string FullUnit;
		public readonly string ShortUnit;

		public static ValueUnit Per (ValueUnit numerator, ValueUnit denominator)
		{
			string fullUnit = String.Concat (numerator.FullUnit, "/", denominator.FullUnit);
			string shortUnit = String.Concat (numerator.ShortUnit, "/", denominator.ShortUnit); 
			return new ValueUnit (fullUnit, shortUnit);
		}

		private ValueUnit(String unit) 
		{
			FullUnit = unit;
			ShortUnit = unit;
		}

		private ValueUnit(String fullUnit, String shortUnit) 
		{
			FullUnit = fullUnit;
			ShortUnit = shortUnit;
		}
	}
}

