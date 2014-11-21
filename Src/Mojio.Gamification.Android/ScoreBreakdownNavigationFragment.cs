
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
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

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
		}

		/*
		 * Draw user interface and layout of the fragment.
		 * Returns the root of the layout as a View, null if the fragment does not provide a UI.
		 */ 
		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			//inflate the layout for this fragment
			View rootView = inflater.Inflate(Resource.Layout.scoring_frag_layout, container, false);
			this.Activity.Title = Resources.GetStringArray (Resource.Array.pages_array) [Arguments.GetInt (ARG_FRAG_NUMBER)];

			mScoreBreakdownLayout = (LinearLayout) rootView.FindViewById<LinearLayout> (Resource.Id.sbd_breakdownLayout);
			mUserStatsLayout = (LinearLayout)rootView.FindViewById<LinearLayout> (Resource.Id.sbd_statsLinearLayout);
		
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
			overallScoreRow.SetScore (overallScore.Score);
			overallScoreRow.SetRankLabel ("RANK " + overallScore.Rank.ToString ());

			ScoreRowView safetyScoreRow = new ScoreRowView (context);
			ScoreWrapper safetyScore = ScoreWrapper.WrapScore (statsManager.MyStats.safetyScore);
			safetyScoreRow.SetScoreLabel ("SAFETY");
			safetyScoreRow.SetScore (safetyScore.Score);
			safetyScoreRow.SetRankLabel ("RANK " + safetyScore.Rank.ToString ());

			ScoreRowView efficiencyScoreRow = new ScoreRowView (context);
			ScoreWrapper efficiencyScore = ScoreWrapper.WrapScore (statsManager.MyStats.efficiencyScore);
			efficiencyScoreRow.SetScoreLabel ("EFFICIENCY");
			efficiencyScoreRow.SetScore (efficiencyScore.Score);
			efficiencyScoreRow.SetRankLabel ("RANK " + efficiencyScore.Rank.ToString ());

			mScoreBreakdownLayout.AddView (overallScoreRow);
			mScoreBreakdownLayout.AddView (safetyScoreRow);
			mScoreBreakdownLayout.AddView (efficiencyScoreRow);
		}

		private void initializeStatsPanel(Context context)
		{
			UserStats stats = ((GamificationApp)(Activity.Application)).MyStatisticsManager.MyStats;

			UnitValueWrapper totalTrips = UnitValueWrapper.WrapValue (stats.totalTrips, UnitValueWrapper.UnitType.NULL_UNIT);
			UnitValueWrapper totalDistance = UnitValueWrapper.WrapValue (stats.totalDistance, UnitValueWrapper.UnitType.DISTANCE_KM);
			UnitValueWrapper totalDuration = UnitValueWrapper.WrapValue (stats.totalDuration, UnitValueWrapper.UnitType.TIME_S);

			var avgTripDistance = stats.totalDistance / stats.totalTrips;
			var avgTripDuration = stats.totalDuration / stats.totalTrips;
			UnitValueWrapper averageTripDistance = UnitValueWrapper.WrapValue (avgTripDistance, UnitValueWrapper.UnitType.DISTANCE_KM);
			UnitValueWrapper averageTripDuration = UnitValueWrapper.WrapValue (avgTripDuration, UnitValueWrapper.UnitType.TIME_S);

			UnitValueWrapper totalHardAccelerations = UnitValueWrapper.WrapValue (stats.totalHardAccelerations, UnitValueWrapper.UnitType.NULL_UNIT);
			UnitValueWrapper totalHardBrakes = UnitValueWrapper.WrapValue (stats.totalHardBrakes, UnitValueWrapper.UnitType.NULL_UNIT);
			UnitValueWrapper totalHardLefts = UnitValueWrapper.WrapValue (stats.totalHardLefts, UnitValueWrapper.UnitType.NULL_UNIT);
			UnitValueWrapper totalHardRights = UnitValueWrapper.WrapValue (stats.totalHardRights, UnitValueWrapper.UnitType.NULL_UNIT);

			var totalHardEvents = stats.totalHardAccelerations + stats.totalHardBrakes + stats.totalHardLefts + stats.totalHardRights;
			var hardEventFrequency = totalHardEvents != 0 ? totalHardEvents / stats.totalDistance : 0;
			UnitValueWrapper freqHardEvents = UnitValueWrapper.WrapValue (hardEventFrequency, UnitValueWrapper.UnitType.Per (UnitValueWrapper.UnitType.NULL_UNIT, UnitValueWrapper.UnitType.DISTANCE_KM));

			UnitValueWrapper totalIdleTime = UnitValueWrapper.WrapValue (stats.totalIdleTime, UnitValueWrapper.UnitType.TIME_S);
			var idleTimePercentage = stats.totalIdleTime != 0 ? (stats.totalIdleTime / stats.totalDuration) * 100 : 0;
			UnitValueWrapper percentageIdleTime = UnitValueWrapper.WrapValue (idleTimePercentage, UnitValueWrapper.UnitType.PERCENTAGE);
			UnitValueWrapper fuelEfficiency = UnitValueWrapper.WrapValue (stats.fuelEfficiency, UnitValueWrapper.UnitType.Per (UnitValueWrapper.UnitType.LITRE, UnitValueWrapper.UnitType.DISTANCE_100KM));
			UnitValueWrapper totalFuelConsumption = UnitValueWrapper.WrapValue (stats.fuelEfficiency * stats.totalDistance / 100, UnitValueWrapper.UnitType.LITRE);

			mUserStatsLayout.AddView (createUserStatRow (context, "Total Trips", totalTrips.GetStringWithShortUnit ()));
			mUserStatsLayout.AddView (createUserStatRow (context, "Total Distance Travelled", totalDistance.GetStringWithShortUnit (2)));
			mUserStatsLayout.AddView (createUserStatRow (context, "Total Time Driven", totalDuration.GetStringWithShortUnit (2)));

			mUserStatsLayout.AddView (createUserStatRow (context, "Average Trip Distance", averageTripDistance.GetStringWithShortUnit (2)));
			mUserStatsLayout.AddView (createUserStatRow (context, "Average Trip Duration", averageTripDuration.GetStringWithShortUnit (2)));

			mUserStatsLayout.AddView (createUserStatRow (context, "Total Hard Accelerations", totalHardAccelerations.GetStringWithShortUnit ()));
			mUserStatsLayout.AddView (createUserStatRow (context, "Total Hard Brakes", totalHardBrakes.GetStringWithShortUnit ()));
			mUserStatsLayout.AddView (createUserStatRow (context, "Total Hard Lefts", totalHardLefts.GetStringWithShortUnit ()));
			mUserStatsLayout.AddView (createUserStatRow (context, "Total Hard Rights", totalHardRights.GetStringWithShortUnit ()));
			mUserStatsLayout.AddView (createUserStatRow (context, "Hard Event Frequency", freqHardEvents.GetStringWithShortUnit (2)));
			mUserStatsLayout.AddView (createUserStatRow (context, "Total Idle Time", totalIdleTime.GetStringWithShortUnit (2)));
			mUserStatsLayout.AddView (createUserStatRow (context, "Idle Time Percentage", percentageIdleTime.GetStringWithShortUnit (2)));
			mUserStatsLayout.AddView (createUserStatRow (context, "Fuel Efficiency", fuelEfficiency.GetStringWithShortUnit (2)));
			mUserStatsLayout.AddView (createUserStatRow (context, "Total Fuel Consumption", totalFuelConsumption.GetStringWithShortUnit (2)));
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

