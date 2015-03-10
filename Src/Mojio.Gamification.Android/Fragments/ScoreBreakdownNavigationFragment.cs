using System;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Graphics;

using Mojio.Gamification.Core;

namespace Mojio.Gamification.Android
{
	public class ScoreBreakdownNavigationFragment : AbstractNavigationFragment
	{
		private LinearLayout mScoreBreakdownLayout;
		private LinearLayout mUserStatsLayout;

		/*
		 * Draw user interface and layout of the fragment.
		 * Returns the root of the layout as a View, null if the fragment does not provide a UI.
		 */ 
		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			//inflate the layout for this fragment
			View rootView = inflater.Inflate(Resource.Layout.scoring_frag_layout, container, false);
			this.Activity.Title = Resources.GetStringArray (Resource.Array.pages_array) [Arguments.GetInt (ARG_FRAG_NUMBER)];

			mScoreBreakdownLayout = rootView.FindViewById<LinearLayout> (Resource.Id.sbd_breakdownLayout);
			mUserStatsLayout = rootView.FindViewById<LinearLayout> (Resource.Id.sbd_statsLinearLayout);
		
			initializeScoreBreakdownPanel (rootView.Context);
			initializeStatsPanel (rootView.Context);
			return rootView;
		}

		private void initializeScoreBreakdownPanel(Context context) 
		{
			StatisticsManager statsManager = ((GamificationApp)(Activity.Application)).MyStatisticsManager;

			ScoreRowView overallScoreRow = new ScoreRowView (context);
			ScoreWrapper overallScore = ScoreWrapper.WrapScore (statsManager.OverallScore);
			overallScoreRow.SetScoreLabel ("OVERALL");
			overallScoreRow.SetScore (overallScore);

			ScoreRowView safetyScoreRow = new ScoreRowView (context);
			ScoreWrapper safetyScore = ScoreWrapper.WrapScore (statsManager.MyStats.safetyScore);
			safetyScoreRow.SetScoreLabel ("SAFETY");
			safetyScoreRow.SetScore (safetyScore);

			ScoreRowView efficiencyScoreRow = new ScoreRowView (context);
			ScoreWrapper efficiencyScore = ScoreWrapper.WrapScore (statsManager.MyStats.efficiencyScore);
			efficiencyScoreRow.SetScoreLabel ("EFFICIENCY");
			efficiencyScoreRow.SetScore (efficiencyScore);

			mScoreBreakdownLayout.AddView (overallScoreRow);
			mScoreBreakdownLayout.AddView (safetyScoreRow);
			mScoreBreakdownLayout.AddView (efficiencyScoreRow);
		}

		private void initializeStatsPanel(Context context)
		{
			UserStatsModel stats = ((GamificationApp)(Activity.Application)).MyStatisticsManager.MyStats;

			string totalTrips = NumberDisplayer.CreateNumberDisplayer (stats.totalTrips, NumberDisplayer.NumberType.INTEGER).GetString ();
			string totalDistance = NumberDisplayer.CreateNumberDisplayer (stats.totalDistance, NumberDisplayer.NumberType.DISTANCE).GetString ();
			string totalDuration = NumberDisplayer.CreateNumberDisplayer (stats.totalDuration, NumberDisplayer.NumberType.TIME).GetString ();

			var avgTripDistance = stats.totalTrips != 0 ? stats.totalDistance / stats.totalTrips : 0;
			var avgTripDuration = stats.totalTrips != 0 ? stats.totalDuration / stats.totalTrips : 0;
			string averageTripDistance = NumberDisplayer.CreateNumberDisplayer (avgTripDistance, NumberDisplayer.NumberType.DISTANCE).GetString ();
			string averageTripDuration = NumberDisplayer.CreateNumberDisplayer (avgTripDuration, NumberDisplayer.NumberType.TIME).GetString ();

			string totalHardAccelerations = NumberDisplayer.CreateNumberDisplayer (stats.totalHardAccelerations, NumberDisplayer.NumberType.INTEGER).GetString ();
			string totalHardBrakes = NumberDisplayer.CreateNumberDisplayer (stats.totalHardBrakes, NumberDisplayer.NumberType.INTEGER).GetString ();
			string totalHardLefts = NumberDisplayer.CreateNumberDisplayer (stats.totalHardLefts, NumberDisplayer.NumberType.INTEGER).GetString ();
			string totalHardRights = NumberDisplayer.CreateNumberDisplayer (stats.totalHardRights, NumberDisplayer.NumberType.INTEGER).GetString ();
			string totalAccidents = NumberDisplayer.CreateNumberDisplayer (stats.totalAccidents, NumberDisplayer.NumberType.INTEGER).GetString ();

			var totalHardEvents = stats.totalHardAccelerations + stats.totalHardBrakes + stats.totalHardLefts + stats.totalHardRights;
			var hardEventFrequency = totalHardEvents / stats.totalDistance;
			string freqHardEvents = new NumberDisplayer (hardEventFrequency, NumberDisplayer.DEFAULT_DECIMAL_PLACES, ValueUnit.Per (ValueUnit.NULL_UNIT, ValueUnit.DISTANCE_KM)).GetString ();

			var accidentFrequency = stats.totalAccidents / stats.totalDistance;
			string freqAccidents = new NumberDisplayer (accidentFrequency, NumberDisplayer.DEFAULT_DECIMAL_PLACES, ValueUnit.Per (ValueUnit.NULL_UNIT, ValueUnit.DISTANCE_KM)).GetString ();

			string totalFuelConsumption = NumberDisplayer.CreateNumberDisplayer (stats.totalFuelConsumption, NumberDisplayer.NumberType.FUEL_CONSUMPTION).GetString ();
			string fuelEfficiency = new NumberDisplayer(stats.totalFuelConsumption * 100 / stats.totalDistance, NumberDisplayer.DEFAULT_DECIMAL_PLACES, ValueUnit.Per (ValueUnit.LITRE, ValueUnit.DISTANCE_100KM)).GetString ();

			mUserStatsLayout.AddView (createUserStatRow (context, "Total Trips", totalTrips));
			mUserStatsLayout.AddView (createUserStatRow (context, "Total Distance Travelled", totalDistance));
			mUserStatsLayout.AddView (createUserStatRow (context, "Total Time Driven", totalDuration));

			mUserStatsLayout.AddView (createUserStatRow (context, "Average Trip Distance", averageTripDistance));
			mUserStatsLayout.AddView (createUserStatRow (context, "Average Trip Duration", averageTripDuration));

			mUserStatsLayout.AddView (createUserStatRow (context, "Total Hard Accelerations", totalHardAccelerations));
			mUserStatsLayout.AddView (createUserStatRow (context, "Total Hard Brakes", totalHardBrakes));
			mUserStatsLayout.AddView (createUserStatRow (context, "Total Hard Lefts", totalHardLefts));
			mUserStatsLayout.AddView (createUserStatRow (context, "Total Hard Rights", totalHardRights));
			mUserStatsLayout.AddView (createUserStatRow (context, "Hard Event Frequency", freqHardEvents));
			mUserStatsLayout.AddView (createUserStatRow (context, "Total Accidents", totalAccidents));
			mUserStatsLayout.AddView (createUserStatRow (context, "Accident Frequency", freqAccidents)); 
			mUserStatsLayout.AddView (createUserStatRow (context, "Fuel Efficiency", fuelEfficiency));
			mUserStatsLayout.AddView (createUserStatRow (context, "Total Fuel Consumption", totalFuelConsumption));
		}

		private LinearLayout createUserStatRow(Context context, string label, string value)
		{
			LinearLayout rowLayout = new LinearLayout(context);
			rowLayout.Orientation = Orientation.Horizontal;

			TextView statLabelTextView = new TextView (context);
			statLabelTextView.Text = label + ":";
			statLabelTextView.SetAllCaps (true);
			statLabelTextView.SetTypeface (null, TypefaceStyle.Bold);
			statLabelTextView.Gravity = GravityFlags.Left | GravityFlags.CenterVertical;
			statLabelTextView.LayoutParameters = new LinearLayout.LayoutParams (LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent, 3);

			TextView statValueTextView = new TextView (context);
			statValueTextView.Text = value;
			statValueTextView.SetTypeface (null, TypefaceStyle.Bold);
			statValueTextView.Gravity = GravityFlags.Right | GravityFlags.CenterVertical;

			rowLayout.AddView (statLabelTextView);
			rowLayout.AddView (statValueTextView); 

			return rowLayout;
		}
	}
}

