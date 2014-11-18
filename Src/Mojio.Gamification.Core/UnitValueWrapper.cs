using System;

namespace Mojio.Gamification.Core
{
	public class UnitValueWrapper
	{
		private UnitType mUnit;
		private double mValue;

		public static UnitValueWrapper WrapValue (int value, UnitType unitType)
		{
			return new UnitValueWrapper (value, unitType);
		}

		public static UnitValueWrapper WrapValue (double value, UnitType unitType)
		{
			return new UnitValueWrapper (value, unitType);
		}

		private UnitValueWrapper (int value, UnitType unitType)
		{
			mValue = value;
			mUnit = unitType;
		}

		private UnitValueWrapper (double value, UnitType unitType)
		{
			mValue = value;
			mUnit = unitType;
		}

		public String GetValueString()
		{
			return mValue.ToString ();
		}
	
		public String GetStringWithFullUnit ()
		{
			return GetValueString () + " " + mUnit.FullUnit;
		}

		public String GetStringWithShortUnit ()
		{
			return GetValueString () + " " + mUnit.ShortUnit;
		}


		public sealed class UnitType 
		{
			public static readonly UnitType NULL_UNIT = new UnitType(String.Empty);
			public static readonly UnitType DISTANCE_KM = new UnitType("kilometers", "km");
			public static readonly UnitType TIME_S = new UnitType("seconds", "s");
			public static readonly UnitType LITRE = new UnitType("litre", "L");
			public static readonly UnitType PERCENTAGE = new UnitType("%");

			public readonly string FullUnit;
			public readonly string ShortUnit;

			private UnitType(String unit) 
			{
				FullUnit = unit;
				ShortUnit = unit;
			}

			private UnitType(String fullUnit, String shortUnit) 
			{
				FullUnit = fullUnit;
				ShortUnit = shortUnit;
			}
		}
	}
}

