using System;

namespace Mojio.Gamification.Core
{
	public class NumberDisplayer
	{
		public enum NumberType
		{
			INTEGER,
			DISTANCE,
			TIME,
			PERCENTAGE,
			FUEL_CONSUMPTION
		}

		public static readonly string NOT_AVAILABLE = "N/A";
		public static readonly int DEFAULT_DECIMAL_PLACES = 2;
		public static readonly int NO_DECIMAL_PLACES = 0;

		protected double _value;
		protected ValueUnit _unit;
		protected int _decimalPlaces;
		protected bool _isNaN;

		/*
		 * Return NumberDisplayer using the default decimal places
		 */
		public static NumberDisplayer CreateNumberDisplayer (double value, NumberType type)
		{
			switch (type) {
			case NumberType.TIME:				return new TimeDisplayer (value);
			case NumberType.PERCENTAGE:			return new PercentageDisplayer (value);
			case NumberType.DISTANCE:			return new DistanceDisplayer (value);
			case NumberType.FUEL_CONSUMPTION:	return new NumberDisplayer (value, DEFAULT_DECIMAL_PLACES, ValueUnit.LITRE);
			default: 							return new NumberDisplayer (value);
			}
		}

		public NumberDisplayer (double value) : this (value, NO_DECIMAL_PLACES, ValueUnit.NULL_UNIT)
		{
		}

		public NumberDisplayer (double value, int decimalPlaces) : this (value, decimalPlaces, ValueUnit.NULL_UNIT)
		{
		}

		public NumberDisplayer (double value, int decimalPlaces, ValueUnit unit)
		{
			_value = value;
			_isNaN = Double.IsNaN (value);
			_unit = unit;
			_decimalPlaces = decimalPlaces;
		}

		public virtual string GetValueString()
		{
			return getFormattedString (_value);
		}

		public string GetStringWithFullUnit ()
		{
			return _isNaN ? NOT_AVAILABLE : GetValueString () + " " + _unit.FullUnit;
		}

		public string GetString ()
		{
			return _isNaN ? NOT_AVAILABLE : GetValueString () + " " + _unit.ShortUnit;
		}

		protected virtual string getFormattedString (double value)
		{
			return _isNaN ? NOT_AVAILABLE : string.Format (getDecimalFormatString (), value);
		}

		protected virtual string getDecimalFormatString ()
		{
			return String.Concat ("{0:F", _decimalPlaces, "}");
		}
	}

	public class DistanceDisplayer : NumberDisplayer
	{
		private bool _isMeters;

		public DistanceDisplayer (double km)
			: base (km)
		{
			_isMeters = km < 1;
			_unit = _isMeters ? ValueUnit.DISTANCE_M : ValueUnit.DISTANCE_KM;
			_decimalPlaces = _isMeters ? NO_DECIMAL_PLACES : DEFAULT_DECIMAL_PLACES;
		}

		public override string GetValueString()
		{
			double value = _isMeters ? _value * 1000 : _value;
			return getFormattedString (value);
		}
	}

	public class TimeDisplayer : NumberDisplayer
	{
		public TimeDisplayer (double seconds)
			: base(seconds, NO_DECIMAL_PLACES)
		{
		}

		public override string GetValueString()
		{
			TimeSpan t = TimeSpan.FromSeconds (_value);
			return string.Format ("{0:D2}h:{1:D2}m:{2:D2}s", t.Hours, t.Minutes, t.Seconds);
		}
	}

	public class PercentageDisplayer : NumberDisplayer
	{
		public PercentageDisplayer (double fraction)
			: base (fraction, DEFAULT_DECIMAL_PLACES, ValueUnit.PERCENTAGE)
		{
		}

		public PercentageDisplayer (double fraction, int decimalPlaces)
			: base (fraction, decimalPlaces, ValueUnit.PERCENTAGE)
		{
		}

		public override string GetValueString()
		{
			double value = _value * 100;
			return getFormattedString (value);
		}
	}
}

