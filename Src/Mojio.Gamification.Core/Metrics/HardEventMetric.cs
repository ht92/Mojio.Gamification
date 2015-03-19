using System;
using System.Collections.Generic;
using System.Linq;
using Mojio.Events;
using Newtonsoft.Json;

namespace Mojio.Gamification.Core
{
	public class HardEventMetric : DistanceBasedMetric
	{
		public IList<HardEvent> Events { get; set; }
		public int Count { get; set; }
	
		public enum HardEventSeverity { Minor,	Major, Severe };

		public static HardEventMetric CreateMetric(IList<Event> hardEvents, double distance)
		{
			return new HardEventMetric (hardEvents, distance);
		}

		[JsonConstructor]
		public HardEventMetric () 
			: base () 
		{
		}

		private HardEventMetric (IList<Event> hardEvents, double distance) 
			: base(distance)
		{
			Events = hardEvents.Where (entry => entry.EventType.Equals (EventType.HardAcceleration)
				|| entry.EventType.Equals (EventType.HardBrake)
				|| entry.EventType.Equals (EventType.HardLeft)
				|| entry.EventType.Equals (EventType.HardRight))
				.ToList ()
				.Cast<HardEvent> ()
				.ToList ();
			Count = Events.Count;
			Measure = Count / TripDistance;
		}

		public int GetHardAccelerationCount ()
		{
			return getCountByType (EventType.HardAcceleration);
		}

		public int GetHardBrakeCount ()
		{
			return getCountByType (EventType.HardBrake);
		}

		public int GetHardLeftCount ()
		{
			return getCountByType (EventType.HardLeft);
		}

		public int GetHardRightCount ()
		{
			return getCountByType (EventType.HardRight);
		}

		private int getCountByType (EventType hardEventType) {
			if (hardEventType == EventType.HardAcceleration
			    || hardEventType == EventType.HardBrake
			    || hardEventType == EventType.HardLeft
			    || hardEventType == EventType.HardRight) {
				return Events.Where (entry => entry.EventType.Equals (hardEventType)).Count (); 
			} else {
				throw new ArgumentException (String.Format ("Invalid event type - {0}.", hardEventType));
			}
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
	}
}

