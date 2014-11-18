
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

		private UserStatsRepository mUserStatsRepository;
		private UserStats mUserStats;
		private LinearLayout mScoreBreakdownLayout;
		private LinearLayout mUserStatsLayout;

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			mUserStatsRepository = ((GamificationApp)this.Activity.Application).MyUserStatsRepository;
			mUserStats = mUserStatsRepository.GetUserStats ();
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
			ScoreWrapper safetyScore = ScoreWrapper.WrapScore (mUserStats.safetyScore);
			ScoreWrapper efficiencyScore = ScoreWrapper.WrapScore (mUserStats.efficiencyScore);
			ScoreWrapper overallScore = ScoreCalculator.CalculateOverallScore (new List<ScoreWrapper> {safetyScore, efficiencyScore});

			ScoreRowView overallScoreRow = new ScoreRowView (context);
			overallScoreRow.SetScoreLabel ("OVERALL");
			overallScoreRow.SetScore (overallScore.Score);
			overallScoreRow.SetRankLabel ("RANK " + overallScore.Rank.ToString ());

			ScoreRowView safetyScoreRow = new ScoreRowView (context);
			safetyScoreRow.SetScoreLabel ("SAFETY");
			safetyScoreRow.SetScore (safetyScore.Score);
			safetyScoreRow.SetRankLabel ("RANK " + safetyScore.Rank.ToString ());

			ScoreRowView efficiencyScoreRow = new ScoreRowView (context);
			efficiencyScoreRow.SetScoreLabel ("EFFICIENCY");
			efficiencyScoreRow.SetScore (efficiencyScore.Score);
			efficiencyScoreRow.SetRankLabel ("RANK " + efficiencyScore.Rank.ToString ());

			mScoreBreakdownLayout.AddView (overallScoreRow);
			mScoreBreakdownLayout.AddView (safetyScoreRow);
			mScoreBreakdownLayout.AddView (efficiencyScoreRow);
		}

		private void initializeStatsPanel(Context context)
		{
			UnitValueWrapper totalDistance = UnitValueWrapper.WrapValue (mUserStats.totalDistance, UnitValueWrapper.UnitType.DISTANCE_KM);
			UnitValueWrapper totalDuration = UnitValueWrapper.WrapValue (mUserStats.totalDuration, UnitValueWrapper.UnitType.TIME_S);
			UnitValueWrapper totalHardEvents = UnitValueWrapper.WrapValue (mUserStats.numHardEvents, UnitValueWrapper.UnitType.NULL_UNIT);
			UnitValueWrapper freqHardEvents = UnitValueWrapper.WrapValue (mUserStats.numHardEvents / mUserStats.totalDistance, UnitValueWrapper.UnitType.NULL_UNIT);
			UnitValueWrapper totalIdleTime = UnitValueWrapper.WrapValue (mUserStats.totalIdleTime, UnitValueWrapper.UnitType.TIME_S);
			UnitValueWrapper percentageIdleTime = UnitValueWrapper.WrapValue (100 * mUserStats.totalIdleTime / mUserStats.totalDuration, UnitValueWrapper.UnitType.PERCENTAGE);
			UnitValueWrapper totalFuelConsumption = UnitValueWrapper.WrapValue (mUserStats.totalFuelConsumption, UnitValueWrapper.UnitType.LITRE);
			UnitValueWrapper fuelEfficiency = UnitValueWrapper.WrapValue (100 * mUserStats.totalFuelConsumption / mUserStats.totalDistance, UnitValueWrapper.UnitType.NULL_UNIT);

			mUserStatsLayout.AddView (createUserStatRow (context, "Total Distance Travelled", totalDistance.GetStringWithShortUnit ()));
			mUserStatsLayout.AddView (createUserStatRow (context, "Total Time Driven", totalDuration.GetStringWithShortUnit ()));
			mUserStatsLayout.AddView (createUserStatRow (context, "Total Hard Events", totalHardEvents.GetStringWithShortUnit ()));
			mUserStatsLayout.AddView (createUserStatRow (context, "Hard Event Frequency", freqHardEvents.GetStringWithShortUnit ()));
			mUserStatsLayout.AddView (createUserStatRow (context, "Total Idle Time", totalIdleTime.GetStringWithShortUnit ()));
			mUserStatsLayout.AddView (createUserStatRow (context, "Idle Time Percentage", percentageIdleTime.GetStringWithShortUnit ()));
			mUserStatsLayout.AddView (createUserStatRow (context, "Total Fuel Consumption", totalFuelConsumption.GetStringWithShortUnit ()));
			mUserStatsLayout.AddView (createUserStatRow (context, "Fuel Efficiency", fuelEfficiency.GetStringWithShortUnit ()));

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

