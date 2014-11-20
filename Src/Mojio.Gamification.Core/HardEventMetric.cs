using System;
using System.Collections.Generic;
using System.Linq;
using Mojio.Events;

namespace Mojio.Gamification.Core
{
	public abstract class HardEventMetric : Metric
	{
		public EventType HardEventType { get; private set; }
		public IList<HardEvent> Events { get; private set; }
		public int Count { get; private set; }
	
		public enum HardEventSeverity { Minor,	Major, Severe };

		public static HardEventMetric CreateMetric(EventType type, IList<Event> hardEvents, double distance)
		{
			switch (type) {
			case EventType.HardAcceleration:
				return new HardAccelerationMetric (hardEvents, distance);
			case EventType.HardBrake:
				return new HardBrakeMetric (hardEvents, distance);
			case EventType.HardLeft:
				return new HardLeftMetric (hardEvents, distance);
			case EventType.HardRight:
				return new HardRightMetric (hardEvents, distance); 
			default:
				throw new ArgumentException ("Illegal EventType: " + type.ToString ());
			}
		}

		public HardEventMetric (EventType type, IList<Event> hardEvents, double distance) 
			: base(distance)
		{
			HardEventType = type;
			Events = hardEvents.Where (entry => entry.EventType.Equals (type))
				.ToList ()
				.Cast<HardEvent> ()
				.ToList ();
			Count = Events.Count;
			Measure = Count / TripDistance;
		}

		private int getCountBySeverity (HardEventSeverity severity)
		{
			var eventsBySeverity = Events.Where (entry => getSeverity (entry).Equals (severity)).ToList ();
			return eventsBySeverity.Count;
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
			public HardAccelerationMetric (IList<Event> hardEvents, double distance)
				: base (EventType.HardAcceleration, hardEvents, distance)
			{
			}
		}

		public class HardBrakeMetric : HardEventMetric
		{
			public HardBrakeMetric (IList<Event> hardEvents, double distance)
				: base (EventType.HardBrake, hardEvents, distance)
			{
			}
		}

		public class HardLeftMetric : HardEventMetric
		{
			public HardLeftMetric (IList<Event> hardEvents, double distance)
				: base (EventType.HardLeft, hardEvents, distance)
			{
			}
		}

		public class HardRightMetric : HardEventMetric
		{
			public HardRightMetric (IList<Event> hardEvents, double distance)
				: base (EventType.HardRight, hardEvents, distance)
			{
			}
		}
	}
}

