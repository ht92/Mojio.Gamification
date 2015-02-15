using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

using Mojio.Events;
using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	public class DiagnosticNavigationFragment : AbstractNavigationFragment
	{
		private Button mFetchButton;
		private Button mAddTripDataButton;
		private Button mResetDataButton;
		private Random rnd = new Random ();

		/*
		 * Draw user interface and layout of the fragment.
		 * Returns the root of the layout as a View, null if the fragment does not provide a UI.
		 */ 
		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			//inflate the layout for this fragment
			View rootView = inflater.Inflate(Resource.Layout.diagnostic_frag_layout, container, false);
			this.Activity.Title = Resources.GetStringArray (Resource.Array.pages_array) [Arguments.GetInt (ARG_FRAG_NUMBER)];

			mFetchButton = (Button) rootView.FindViewById<Button> (Resource.Id.diag_fetchButton);
			mAddTripDataButton = (Button)rootView.FindViewById<Button> (Resource.Id.diag_addTripButton);
			mResetDataButton = (Button) rootView.FindViewById<Button> (Resource.Id.diag_resetDataButton);

			mFetchButton.Click += mFetchButton_onClick;
			mAddTripDataButton.Click += mAddTripDataButton_onClick;
			return rootView;
		}

		private async void mFetchButton_onClick (object sender, EventArgs e)
		{
			var tripData = await GamificationApp.GetInstance ().MyConnectionService.FetchLatestTripAsync ();
			((GamificationApp)Activity.Application).MyStatisticsManager.AddTrip (tripData.Item1, tripData.Item2);
		}

		private void mAddTripDataButton_onClick (object sender, EventArgs e)
		{
			Trip trip = new Trip ();
			trip.Distance = rnd.Next (1, 100);
			trip.StartTime = DateTime.Now;
			trip.EndTime = trip.StartTime.AddMinutes (rnd.Next (1, 100));
			trip.VehicleId = Guid.Empty;
			trip.FuelEfficiency = rnd.Next (7, 15);

			List<Event> events = new List<Event>();
			for (int j = 0; j < 4; j++) {
				int num = rnd.Next (0, 1);
				for (int i = 0; i < num; i++) {
					switch (j) 
					{
					case 0:
						events.Add (new HardEvent (EventType.HardAcceleration));
						break;
					case 1:
						events.Add (new HardEvent (EventType.HardBrake));
						break;
					case 2:
						events.Add (new HardEvent (EventType.HardLeft));
						break;
					case 3:
						events.Add (new HardEvent (EventType.HardRight));
						break;
					}
				}
			}
			((GamificationApp)Activity.Application).MyStatisticsManager.AddTrip (trip, events);
		}
			
	}
}

