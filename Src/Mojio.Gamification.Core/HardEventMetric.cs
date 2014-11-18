using System;
using System.Collections.Generic;
using System.Linq;
using Mojio.Events;

namespace Mojio.Gamification.Core
{
	public abstract class HardEventMetric : Metric
	{
		private IList<HardEvent> mHardEvents;
		public int Count { get; private set; }

		public enum HardEventSeverity { Minor,	Major, Severe };

		public static HardEventMetric CreateMetric(EventType type, IList<HardEvent> hardEvents, double distance)
		{
			switch (type) {
			case EventType.HardAcceleration:
				return new HardAccelerationMetric (hardEvents, distance);
			case EventType.HardBrake:
				return new HardBrakeMetric (hardEvents, distance);
			case EventType.HardLeft:
			case EventType.HardRight:
				return new HardTurnMetric (hardEvents, distance);
			default:
				throw new ArgumentException ("Illegal EventType: " + type.ToString ());
			}
		}

		public HardEventMetric (IList<HardEvent> hardEvents, double distance) 
			: base(distance)
		{
			mHardEvents = hardEvents;
		}

		protected double getHardEventFrequency (EventType type)
		{
			var hardEventCount = getHardEventCount (type);
			double hardEventFrequency = hardEventCount / TripDistance;
			return hardEventFrequency;
		}

		protected int getHardEventCount (EventType type)
		{
			int count = 0; 
			foreach (HardEventSeverity severity in Enum.GetValues (typeof(HardEventSeverity))) {
				count += getHardEventCount (type, severity);
			}
			return count;
		}

		protected int getHardEventCount (EventType type, HardEventSeverity severity)
		{
			var hardEvents = mHardEvents.Where (hardEvent => hardEvent.Type.Equals (type) && getSeverity (hardEvent) == severity).ToList ();
			return hardEvents.Count;
		}

		//−0.30 to −0.39 minor
		//−0.40 to −0.49 major
		//−0.5 to −0.59 severe
		//TODO: Probably need to override this in the future
		protected virtual HardEventSeverity getSeverity(HardEvent hardEvent)
		{
			double force = Math.Abs (hardEvent.Force);
			if (force >= 0.5)
				return HardEventSeverity.Severe;
			else if (force >= 0.4)
				return HardEventSeverity.Major;
			else 
				return HardEventSeverity.Minor;
		}

		public class HardAccelerationMetric : HardEventMetric
		{
			public HardAccelerationMetric (IList<HardEvent> hardEvents, double distance)
				: base (hardEvents, distance)
			{
				Count = getHardEventCount (EventType.HardAcceleration);
				Measure = getHardEventFrequency (EventType.HardAcceleration);
			}
		}

		public class HardBrakeMetric : HardEventMetric
		{
			public HardBrakeMetric (IList<HardEvent> hardEvents, double distance)
				: base (hardEvents, distance)
			{
				Count = getHardEventCount (EventType.HardBrake);
				Measure = getHardEventFrequency (EventType.HardBrake);
			}
		}

		public class HardTurnMetric : HardEventMetric
		{
			public HardTurnMetric (IList<HardEvent> hardEvents, double distance)
				: base (hardEvents, distance)
			{
				Count = getHardEventCount (EventType.HardLeft) + getHardEventCount (EventType.HardRight);
				Measure = getHardEventFrequency (EventType.HardLeft) + getHardEventFrequency (EventType.HardRight);
			}
		}
	}
}

