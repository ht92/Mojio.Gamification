﻿using System;
using System.Collections.Generic;
using System.Linq;
using Mojio.Events;
using Newtonsoft.Json;

namespace Mojio.Gamification.Core
{
	public class AccidentEventMetric : DistanceBasedMetric
	{
		public IList<Event> Events { get; private set; }
		public int Count { get; private set; }
		public int Weight { get; private set; }

		private static int MAX_WEIGHT = 100;
		private static int MIN_WEIGHT = 1;

		public static AccidentEventMetric CreateMetric(IList<Event> accidentEvents, double distance)
		{
			return new AccidentEventMetric (accidentEvents, distance);
		}

		[JsonConstructor]
		public AccidentEventMetric () 
			: base () 
		{
		}

		private AccidentEventMetric (IList<Event> accidentEvents, double distance) 
			: base(distance)
		{
			Events = accidentEvents.Where (entry => entry.EventType.Equals (EventType.Accident)).ToList ();
			Count = Events.Count;
			Measure = Count / TripDistance;
			Weight = Count > 0 ? MAX_WEIGHT : MIN_WEIGHT;
		}
	}
}

